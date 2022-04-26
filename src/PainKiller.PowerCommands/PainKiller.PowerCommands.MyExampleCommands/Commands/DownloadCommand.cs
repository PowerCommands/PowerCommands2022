using PainKiller.HttpClientUtils;
using PainKiller.PowerCommands.Core.Managers;
using PainKiller.PowerCommands.Security.DomainObjects;
using PainKiller.PowerCommands.Shared.DomainObjects.Configuration;
namespace PainKiller.PowerCommands.MyExampleCommands.Commands;

[Tags("download|checksum|example")]
[PowerCommand(     description: "Download a file that is provides as an argument, after download you get a checksum of the downloaded file",
                     arguments: "url:<url>",
             argumentMandatory: true,
                         qutes: "path:<save file path>", 
                qutesMandatory: true,
                    suggestion: "https://downloadurl.com \"filename.txt\"",
                       example: "download https://downloadurl.com \"filename.txt\"",
                      useAsync: true)]
public class DownloadCommand : CommandBase<CommandsConfiguration>
{
    private ProgressBar? _progressbar;
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
        return CreateRunResult(input);
    }
    private bool ProgressChanged(long? totalBytes, long totalBytesRead, double? percentage)
    {
        _progressbar ??= new ProgressBar(totalBytes ?? 0);
        _progressbar.Update(totalBytesRead);
        _progressbar.Show();
        if (totalBytes != totalBytesRead) return false;
        
        var fileCheckSum = new FileChecksum(_fileName);
        WriteLine($"Download content from {_downloadUrl} to file {_fileName} completed. Checksum: {fileCheckSum.Mde5Hash}");
        Console.Write("\npcm>");
        _progressbar = null;
        return false;
    }
}