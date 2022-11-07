using System.Net.Http.Json;
namespace PainKiller.PowerCommands.Core.Services;
public class DocsDBService : IDocsDBService
{
    private readonly ArtifactPathsConfiguration _artifact;
    private DocsDBService() => _artifact = ConfigurationService.Service.Get<ArtifactPathsConfiguration>().Configuration;
    private static readonly Lazy<IDocsDBService> Lazy = new(() => new DocsDBService());
    public static IDocsDBService Service => Lazy.Value;
    public void MergeDocsDB()
    {
        try
        {
            var httpClient = new HttpClient();
            var uri = _artifact.DocsDbGithub;
            var newDocsDB = httpClient.GetFromJsonAsync<DocsDB?>(uri).Result;
            if (newDocsDB == null)
            {
                ConsoleService.WriteError(nameof(DocsDBService),$"Could not download {nameof(DocsDB)} from {uri}");
                return;
            }
            ConsoleService.WriteLine(nameof(DocsDBService),$"Downloaded latest available {nameof(DocsDB)} from {uri} OK");
            var currentDocsDB = StorageService<DocsDB>.Service.GetObject();
            ConsoleService.WriteLine(nameof(DocsDBService), $"Merging changes (if any) in {nameof(DocsDB)}");
            ConsoleService.WriteLine(nameof(DocsDBService), $"Local DB items count: {currentDocsDB.Docs.Count}");
            ConsoleService.WriteLine(nameof(DocsDBService), $"New   DB items count: {newDocsDB.Docs.Count}");

            var hasChanges = false;
            foreach (var doc in newDocsDB.Docs)
            {
                if (currentDocsDB.Docs.Any(d => d.Name == doc.Name && d.Tags == doc.Tags && d.Uri == doc.Uri)) continue;
                hasChanges = true;
                var needsUpdate = currentDocsDB.Docs.FirstOrDefault(d => d.Name == doc.Name && d.Updated < doc.Updated);
                if (needsUpdate != null)
                {
                    ConsoleService.WriteLine(nameof(DocsDBService), $"{needsUpdate.Name} has change and the new changes will be updated in {nameof(DocsDB)}");
                    needsUpdate.Tags = doc.Tags;
                    needsUpdate.Uri = doc.Uri;
                    needsUpdate.Updated = DateTime.Now;
                    needsUpdate.Version = +1;
                    currentDocsDB.Docs.Remove(needsUpdate);
                    currentDocsDB.Docs.Add(needsUpdate);
                    continue;
                }
                currentDocsDB.Docs.Add(doc);
                ConsoleService.WriteLine(nameof(DocsDBService), $"{doc.Name} has been added to the {nameof(DocsDB)}");
            }
            if (hasChanges)
            {
                var fileName = StorageService<DocsDB>.Service.StoreObject(currentDocsDB);
                ConsoleService.WriteLine(nameof(DocsDBService), $"\nThe changes has been merged with your local {nameof(DocsDB)} file and saved to file [{fileName}]");
                return;
            }
            ConsoleService.WriteLine(nameof(DocsDBService), $"Your local {nameof(DocsDB)} is already up to date with the latest version, no changes made.");
        }
        catch (Exception e)
        {
            ConsoleService.WriteError(nameof(DocsDBService), $"Error occurred, the status of the merge between the local {nameof(DocsDB)} and the latest one is unknown, you could delete the local one and try update again.");
        }
    }
}