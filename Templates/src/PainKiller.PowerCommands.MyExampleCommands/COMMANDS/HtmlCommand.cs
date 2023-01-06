using PainKiller.PowerCommands.Shared.DomainObjects.Configuration;

namespace $safeprojectname$.Commands;

public class HtmlCommand : CommandBase<CommandsConfiguration>
{
    public HtmlCommand(string identifier, CommandsConfiguration configuration) : base(identifier, configuration, HtmlConsoleService.Service) { }

    public override RunResult Run()
    {
        WriteHeadLine("Headline");
        WriteCritical("Critical");
        WriteFailure("Failure");
        WriteSuccess("Success");
        WriteLine("Standard line output");
        DisplayOutput();
        return Ok();
    }
}