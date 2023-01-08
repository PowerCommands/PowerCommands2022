namespace PainKiller.PowerCommands.MyExampleCommands.Commands;

[PowerCommandDesign(description: "Zip files of a given path, filter could be use to select only certain files that matches the filter",
                      arguments: "<directory>",
                         quotes: "<directory>",
                        options: "!filter|!output",
                  example: "zip \"c:\\temp\"|zip \"c:\\temp\" --filter *.txt")]
public class ZipCommand : CommandBase<CommandsConfiguration>
{
    public ZipCommand(string identifier, CommandsConfiguration configuration) : base(identifier, configuration) { }
    public override RunResult Run()
    {
        if (string.IsNullOrEmpty(Input.Path)) return BadParameterError("A valid path must be provided as argument");

        var filter = GetOptionValue("filter");
        WriteHeadLine($"Zipping files in directory: {Input.Path}...");
        
        var zipResult = ZipService.Service.ArchiveFilesInDirectory(Input.Path, "example", useTimestampSuffix: true, filter: string.IsNullOrEmpty(filter) ? "*" : filter);
        Console.WriteLine();
        
        WriteHeadLine("Result");
        WriteCodeExample(nameof(zipResult.Path), $"{zipResult.Path}");
        WriteCodeExample(nameof(zipResult.FileCount), $"{zipResult.FileCount}");
        WriteCodeExample(nameof(zipResult.FileSizeUncompressedInKb), $"{zipResult.FileSizeUncompressedInKb}");
        WriteCodeExample(nameof(zipResult.FileSizeCompressedInKb), $"{zipResult.FileSizeUncompressedInKb}");
        WriteCodeExample(nameof(zipResult.Checksum), $"{zipResult.Checksum}");

        if(zipResult.HasException) WriteError(zipResult.ExceptionMessage);
        return Ok();
    }
}