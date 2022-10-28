using PainKiller.PowerCommands.MyExampleCommands.Configuration;
using PainKiller.PowerCommands.Security.DomainObjects;

namespace PainKiller.PowerCommands.MyExampleCommands.Commands;

[PowerCommand(  description: "Calculate the checksum for a valid file, could be practical when you distribute your PowerCommands application",
                qutes: "<Filename>",
                example: "checksum Painkiller.PowerCommands.Core")]
public class ChecksumCommand : CommandBase<PowerCommandsConfiguration>
{
    public ChecksumCommand(string identifier, PowerCommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run()
    {
        var fileName = Input.SingleQuote;
        if (!File.Exists(fileName)) return CreateBadParameterRunResult($"Filename [{fileName}] does not exist");
        var mde5Hash = new FileChecksum(fileName).Mde5Hash;
        WriteHeadLine(mde5Hash);
        return CreateRunResult();
    }
}