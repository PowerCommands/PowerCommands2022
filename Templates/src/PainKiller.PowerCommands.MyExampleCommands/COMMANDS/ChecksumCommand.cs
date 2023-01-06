using PainKiller.PowerCommands.Security.DomainObjects;

namespace $safeprojectname$.Commands;

[PowerCommandTest(tests: "\"Painkiller.PowerCommands.Core.dll\"")]
[PowerCommandDesign(  description: "Calculate the checksum for a valid file, could be practical when you distribute your PowerCommands application",
                           quotes: "!<Filename>",
                          example: "checksum Painkiller.PowerCommands.Core.dll")]
public class ChecksumCommand : CommandBase<PowerCommandsConfiguration>
{
    public ChecksumCommand(string identifier, PowerCommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run()
    {
        var fileName = Input.SingleQuote;
        if (!File.Exists(fileName)) return BadParameterError($"Filename [{fileName}] does not exist");
        var mde5Hash = new FileChecksum(fileName).Mde5Hash;
        WriteHeadLine(mde5Hash);
        return Ok();
    }
}