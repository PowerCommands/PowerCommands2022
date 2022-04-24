using PainKiller.PowerCommands.Core.Extensions;
using PainKiller.PowerCommands.Core.Services;
using PainKiller.PowerCommands.Shared.DomainObjects.Configuration;
namespace PainKiller.PowerCommands.MyExampleCommands.Commands;

[Tags("zip|compression|temp|path")]
[PowerCommand(description: "Zip files of a given path using filter, filter is optional",
    arguments: "path: A valid path to a directory",
    qutes: "filter: optional, what kind of files to be zipped from the given directory.",
    example: "zip c:\\temp|zip c:\\temp \"*.txt\"")]
public class ZipCommand : CommandBase<CommandsConfiguration>
{
    public ZipCommand(string identifier, CommandsConfiguration configuration) : base(identifier, configuration) { }
    public override RunResult Run(CommandLineInput input)
    {
        if (string.IsNullOrEmpty(input.Path)) return CreateBadParameterRunResult(input, "A valid path must be provided as argument");
        WriteHeadLine($"Zipping files in directory: {input.Path}...");
        var zipResult = ZipService.Service.ArchiveFilesInDirectory(input.Path, "example", useTimestampSuffix: true, filter: string.IsNullOrEmpty(input.SingleQuote) ? "*" : input.SingleQuote);
        Console.WriteLine();
        WriteHeadLine("Result", addToOutput: false);
        this.WriteObjectDescription(nameof(zipResult.Path),zipResult.Path!);
        this.WriteObjectDescription(nameof(zipResult.FileCount),zipResult.FileCount.ToString());
        this.WriteObjectDescription(nameof(zipResult.FileSizeUncompressedInKb),zipResult.FileSizeUncompressedInKb.ToString());
        this.WriteObjectDescription(nameof(zipResult.FileSizeCompressedInKb),zipResult.FileSizeCompressedInKb.ToString());
        this.WriteObjectDescription(nameof(zipResult.Checksum),zipResult.Checksum!);
        if(zipResult.HasException) this.WriteObjectDescription(nameof(zipResult.ExceptionMessage),zipResult.ExceptionMessage);
        
        return CreateRunResult(input);
    }
}