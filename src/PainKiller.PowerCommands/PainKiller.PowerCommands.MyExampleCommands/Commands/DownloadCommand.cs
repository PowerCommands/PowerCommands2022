﻿using PainKiller.HttpClientUtils;
using PainKiller.PowerCommands.Core.BaseClasses;
using PainKiller.PowerCommands.Core.Managers;
using PainKiller.PowerCommands.Shared.Attributes;
using PainKiller.PowerCommands.Shared.DomainObjects.Configuration;
using PainKiller.PowerCommands.Shared.DomainObjects.Core;

namespace PainKiller.PowerCommands.MyExampleCommands.Commands;

[PowerCommand(description: "Download a file that is provides as an argument", arguments: "url: Single argument must be a valid URL to something do download",qutes: "Single qute is the filename to be created, a full path could be provided but is not necessary", useAsync: true)]
public class DownloadCommand : CommandBase<CommandsConfiguration>
{
    private ProgressBar? progressbar = null;
    private string _downloadUrl = "";
    private string _fileName = "";
    
    public DownloadCommand(string identifier, CommandsConfiguration configuration) : base(identifier, configuration) { }

    public override async Task RunAsync(CommandLineInput input)
    {
        _downloadUrl = input.SingleArgument;
        _fileName = input.SingleQuote.Replace("\"","");
        
        var retVal =  DownloadWithProgressService.Service.Download(_downloadUrl, _fileName, ProgressChanged);
        await retVal;
    }
    private bool ProgressChanged(long? totalBytes, long totalBytesRead, double? percentage)
    {
        if (progressbar == null) progressbar = new ProgressBar(totalBytes ?? 0);
        progressbar.Update(totalBytesRead);
        progressbar.Show();
        if (totalBytes == totalBytesRead)
        {
            WriteLine($"Download content from {_downloadUrl} to file {_fileName} completed.");
            Console.Write("pcm>");
            progressbar = null;
        }
        return false;
    }
}