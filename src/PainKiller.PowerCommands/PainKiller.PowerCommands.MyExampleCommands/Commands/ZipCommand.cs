namespace PainKiller.PowerCommands.MyExampleCommands.Commands;

[PowerCommandDesign(description: "Zip files of a given path, filter could be use to select only certain files that matches the filter",
                arguments: "!<directory>",
                    quotes: "<filter>",
                  example: "zip c:\\temp|zip c:\\temp \"*.txt\"")]
public class ZipCommand : CommandBase<CommandsConfiguration>
{
    public ZipCommand(string identifier, CommandsConfiguration configuration) : base(identifier, configuration) { }
    public override RunResult Run()
    {
        if (string.IsNullOrEmpty(Input.Path)) return BadParameterError("A valid path must be provided as argument");
        WriteHeadLine($"Zipping files in directory: {Input.Path}...");
        var zipResult = ZipService.Service.ArchiveFilesInDirectory(Input.Path, "example", useTimestampSuffix: true, filter: string.IsNullOrEmpty(Input.SingleQuote) ? "*" : Input.SingleQuote);
        Console.WriteLine();
        WriteHeadLine("Result", addToOutput: false);
        ConsoleService.Service.WriteObjectDescription($"{GetType().Name}", nameof(zipResult.Path),zipResult.Path!);
        ConsoleService.Service.WriteObjectDescription($"{GetType().Name}",nameof(zipResult.FileCount),zipResult.FileCount.ToString());
        ConsoleService.Service.WriteObjectDescription($"{GetType().Name}",nameof(zipResult.FileSizeUncompressedInKb),zipResult.FileSizeUncompressedInKb.ToString());
        ConsoleService.Service.WriteObjectDescription($"{GetType().Name}",nameof(zipResult.FileSizeCompressedInKb),zipResult.FileSizeCompressedInKb.ToString());
        ConsoleService.Service.WriteObjectDescription($"{GetType().Name}", nameof(zipResult.Checksum),zipResult.Checksum!);
        if(zipResult.HasException) ConsoleService.Service.WriteObjectDescription($"{GetType().Name}", nameof(zipResult.ExceptionMessage),zipResult.ExceptionMessage);
        
        return Ok();
    }
}