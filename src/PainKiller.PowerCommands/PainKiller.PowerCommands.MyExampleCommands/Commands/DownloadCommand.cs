﻿using PainKiller.HttpClientUtils;
using PainKiller.PowerCommands.Core.BaseClasses;
using PainKiller.PowerCommands.Core.Managers;
using PainKiller.PowerCommands.Security.DomainObjects;
using PainKiller.PowerCommands.Shared.Attributes;
using PainKiller.PowerCommands.Shared.DomainObjects.Configuration;
using PainKiller.PowerCommands.Shared.DomainObjects.Core;
using PainKiller.PowerCommands.Shared.Enums;

namespace PainKiller.PowerCommands.MyExampleCommands.Commands;

[PowerCommand(description: "Download a file that is provides as an argument, after download you get a checksum of the downloaded file",
              arguments: "url: Single argument must be a valid URL to something do download",
              qutes: "Single qute is the filename to be created, a full path could be provided but is not necessary", 
              defaultParameter:"https://downloadurl.com \"filename.txt\"",
              useAsync: true)]
[Tags("download|checksum|example")]
public class DownloadCommand : CommandBase<CommandsConfiguration>
{
    private ProgressBar? progressbar = null;
    private string _downloadUrl = "";
    private string _fileName = "";
    
    public DownloadCommand(string identifier, CommandsConfiguration configuration) : base(identifier, configuration) { }

    public override async Task<RunResult> RunAsync(CommandLineInput input)
    {
        WriteLine("Command executes in async mode, if any error occures you will not see them, a argument must be provided and it must be a valid url to something that could be downloaded.");
        _downloadUrl = input.SingleArgument;
        _fileName = input.SingleQuote;
        
        var retVal =  DownloadWithProgressService.Service.Download(_downloadUrl, _fileName, ProgressChanged);
        await retVal;
        return CreateRunResult(this, input, RunResultStatus.Ok);
    }
    private bool ProgressChanged(long? totalBytes, long totalBytesRead, double? percentage)
    {
        if (progressbar == null) progressbar = new ProgressBar(totalBytes ?? 0);
        progressbar.Update(totalBytesRead);
        progressbar.Show();
        if (totalBytes == totalBytesRead)
        {
            var fileCheckSum = new FileChecksum(_fileName);
            WriteLine($"Download content from {_downloadUrl} to file {_fileName} completed. Checksum: {fileCheckSum.Mde5Hash}");
            Console.Write("\npcm>");
            progressbar = null;
        }
        return false;
    }
}