using PainKiller.PowerCommands.ReadLine;

namespace PainKiller.PowerCommands.MyExampleCommands.Commands;

[PowerCommandTest(tests: "! |!--pause 3")]
[PowerCommandsToolbar(    labels: "[1. Hit space =>]|[2. Hit tab =>]|[3. Enter space and an option value]|[4. Hit enter]",
    colors: new [] { ConsoleColor.DarkBlue,ConsoleColor.DarkGreen ,ConsoleColor.Red,ConsoleColor.DarkYellow})]
[PowerCommandDesign( description: "Description is override in config file",
                         options: "!MANDATORY|!pause",
                         example: "//Must provide the MANDATORY option, will trigger a validation error otherwise|demo MANDATORY|//Test the pause service|demo --pause 5 MANDATORY")]
public class DemoCommand : CommandWithToolbarBase<PowerCommandsConfiguration>
{
    public DemoCommand(string identifier, PowerCommandsConfiguration configuration) : base(identifier, configuration) => ReadLineService.OpenShortCutPressed += OpenShortCutPressed;
    private void OpenShortCutPressed() => WriteSuccessLine("You pressed [CTRL]+[O]");
    public override RunResult Run()
    {
        ToolbarService.ClearToolbar();

        PauseService.Pause(Input);

        WriteHeadLine(" Congratulations! You have setup your PowerCommands solution correctly!\n");
        WriteLine(" You may set up some things in your PowerCommandsConfiguration.yaml file, like the path to your favorite code editor");
        WriteLine(" This [demo] command could now be removed from your solution.\n");
        WriteLine(" The [config] command could also be deleted (practical help commands only)\n");
        WriteLine(" The [dir] command could be kept or moved to the Core/Commands if your PowerCommands console needs directory traversal.\n");
        Write(" Find more example commands on github: ");
        WriteUrl($"{Configuration.Repository}.\n\n");

        var diagnostic = DialogService.YesNoDialog(" Do you want to show diagnostic info from the input?");
        if (!diagnostic)
        {
            Console.Clear();
            return Ok();
        }

        
        WriteHeadLine("Diagnostic output of the command line input, for testing purposes");
        WriteLine($"Raw input: {Input.Raw}");
        WriteLine($"Arguments: {string.Join(' ', Input.Arguments)}");
        WriteLine($"SingleArgument: {string.Join(' ', Input.SingleArgument)}\n");
        WriteLine($"Quotes: {string.Join(' ', Input.Quotes)}");
        WriteLine($"SingleQuote: {string.Join(' ', Input.SingleQuote)}\n");
        WriteHeadLine("Declared Options, with value if any.");
        foreach (var powerOption in Options)
        {
            WriteLine($"Raw input: {powerOption.Raw}");
            WriteLine($"{powerOption.Name} {powerOption.Value} {nameof(powerOption.ValueIsRequired)}: {powerOption.ValueIsRequired}\n");
            WriteLine($"{powerOption.Name} {powerOption.Value} {nameof(powerOption.IsMandatory)}: {powerOption.IsMandatory}\n");
        }
        WriteHeadLine("Input Options, with value if any.");
        foreach (var powerOption in Input.Options)
        {
            WriteLine($"Raw input: {powerOption} Value: {GetOptionValue(powerOption.Replace("--",""))}\n");
        }
        WriteHeadLine("Path value, witch is the first valid path in the input.");
        WriteLine(Input.Path);

        return Ok();
    }
}