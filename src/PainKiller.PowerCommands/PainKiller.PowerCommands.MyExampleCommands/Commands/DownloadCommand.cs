using PainKiller.PowerCommands.Security.DomainObjects;

namespace PainKiller.PowerCommands.MyExampleCommands.Commands;

[PowerCommandDesign(  description: "This text should be overriden by config for demo purposes, it should not work by purpose if the command is not overriden")]
public class DownloadCommand : CommandBase<CommandsConfiguration>
{
    private ProgressBar? _progressbar;
    private string _downloadUrl = "";
    private string _fileName = "";
    public DownloadCommand(string identifier, CommandsConfiguration configuration) : base(identifier, configuration) { }
    public override async Task<RunResult> RunAsync()
    {
        WriteLine("Command executes in async mode, if any error occures you will not see them, a argument must be provided and it must be a valid url to something that could be downloaded.");
        _downloadUrl = Input.SingleArgument;
        _fileName = Input.SingleQuote;
        
        var retVal =  DownloadWithProgressService.Service.Download(_downloadUrl, _fileName, ProgressChanged);
        await retVal;
        return Ok();
    }
    private bool ProgressChanged(long? totalBytes, long totalBytesRead, double? percentage, string downloadUrl, string filePath)
    {
        _progressbar ??= new ProgressBar(totalBytes ?? 0);
        _progressbar.Update(totalBytesRead);
        _progressbar.Show();
        if (totalBytes != totalBytesRead) return false;
        
        var fileCheckSum = new FileChecksum(_fileName);
        WriteLine($"Download content from {_downloadUrl} to file {_fileName} completed. Checksum: {fileCheckSum.Mde5Hash}");
        Console.Write($"\n{Configuration.Prompt}");
        _progressbar = null;
        return false;
    }
}