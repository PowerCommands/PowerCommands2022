﻿using System.Net.Http.Json;
using PainKiller.PowerCommands.Shared.Extensions;

namespace PainKiller.PowerCommands.Core.Services;
public class GithubService : IGithubService
{
    private readonly ArtifactPathsConfiguration _artifact;
    private GithubService() => _artifact = ConfigurationService.Service.Get<ArtifactPathsConfiguration>().Configuration;
    private static readonly Lazy<IGithubService> Lazy = new(() => new GithubService());
    public static IGithubService Service => Lazy.Value;
    public void MergeDocsDB()
    {
        try
        {
            var httpClient = new HttpClient();
            var uri = _artifact.DocsDbGithub;
            var newDocsDB = httpClient.GetFromJsonAsync<DocsDB?>(uri).Result;
            if (newDocsDB == null)
            {
                ConsoleService.WriteError(nameof(GithubService),$"Could not download {nameof(DocsDB)} from {uri}");
                return;
            }
            ConsoleService.WriteLine(nameof(GithubService),$"Downloaded latest available {nameof(DocsDB)} from {uri} OK");
            var currentDocsDB = StorageService<DocsDB>.Service.GetObject();
            ConsoleService.WriteLine(nameof(GithubService), $"Merging changes (if any) in {nameof(DocsDB)}");
            ConsoleService.WriteLine(nameof(GithubService), $"Local DB items count: {currentDocsDB.Docs.Count}");
            ConsoleService.WriteLine(nameof(GithubService), $"New   DB items count: {newDocsDB.Docs.Count}");

            var hasChanges = false;
            foreach (var doc in newDocsDB.Docs)
            {
                if (currentDocsDB.Docs.Any(d => d.Name == doc.Name && d.Tags == doc.Tags && d.Uri == doc.Uri)) continue;
                hasChanges = true;
                var needsUpdate = currentDocsDB.Docs.FirstOrDefault(d => d.Name == doc.Name && d.Updated < doc.Updated);
                if (needsUpdate != null)
                {
                    ConsoleService.WriteLine(nameof(GithubService), $"{needsUpdate.Name} has change and the new changes will be updated in {nameof(DocsDB)}");
                    needsUpdate.Tags = doc.Tags;
                    needsUpdate.Uri = doc.Uri;
                    needsUpdate.Updated = DateTime.Now;
                    needsUpdate.Version = +1;
                    currentDocsDB.Docs.Remove(needsUpdate);
                    currentDocsDB.Docs.Add(needsUpdate);
                    continue;
                }
                currentDocsDB.Docs.Add(doc);
                ConsoleService.WriteLine(nameof(GithubService), $"{doc.Name} has been added to the {nameof(DocsDB)}");
            }
            if (hasChanges)
            {
                var fileName = StorageService<DocsDB>.Service.StoreObject(currentDocsDB);
                ConsoleService.WriteLine(nameof(GithubService), $"\nThe changes has been merged with your local {nameof(DocsDB)} file and saved to file [{fileName}]");
                return;
            }
            ConsoleService.WriteLine(nameof(GithubService), $"Your local {nameof(DocsDB)} is already up to date with the latest version, no changes made.");
        }
        catch (Exception e)
        {
            ConsoleService.WriteError(nameof(GithubService), $"Error occurred, the status of the merge between the local {nameof(DocsDB)} and the latest one is unknown, you could delete the local one and try update again.");
        }
    }
    public void DownloadCommand(string name)
    {
        var commandFileName = $"{name.ToFirstLetterUpper()}Command.cs";
        var httpClient = new HttpClient();
        var uri = $"{_artifact.GithubRoot}/PainKiller.PowerCommands.{_artifact.Name}Commands/Commands/{commandFileName}";
        ConsoleService.WriteLine(nameof(GithubService), $"Downloading file [{uri}]...");
        var fileContent = httpClient.GetStringAsync(uri).Result;
        var solutionRoot = CliManager.GetLocalSolutionRoot();

        File.WriteAllText(Path.Combine(solutionRoot, $"PainKiller.PowerCommands.{_artifact.Name}Commands","Commands", $"{commandFileName}"), fileContent);
        ConsoleService.WriteLine(nameof(GithubService), $"File [{commandFileName}] updated...");
    }
}