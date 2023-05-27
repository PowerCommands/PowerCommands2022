namespace $safeprojectname$.Commands;

[PowerCommandTest(tests: "! |!--pause 3")]
[PowerCommandDesign( description: "Demo command just to try out how you could use the input, do not forget the MANDATORY option, will trigger a validation error otherwise! ;-)\n That is because the option name is typed with UPPERCASE letters, useful when you want a mandatory option\n The pause option on the other hand starts with a ! symbol meaning that if you add the --pause option you must also give it a value, an integer in this case.",
                         options: "MANDATORY|!pause",
                         example: "//Must provide the MANDATORY option will trigger a validation error otherwise|demo MANDATORY|//Test the pause service|demo --pause 5 MANDATORY")]
public class DemoCommand : CommandBase<PowerCommandsConfiguration>
{
    public DemoCommand(string identifier, PowerCommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run()
    {
        PauseService.Pause(Input);

        WriteLine("Hello World!");
        WriteHeadLine("Congratulations! You have setup your PowerCommands solution correctly!");
        WriteLine("You may set up some things in your PowerCommandsConfiguration.yaml file, like the path to your favorite code editor");
        WriteLine("Just type dir and hit enter to open up this applications bin directory");
        WriteLine("");
        WriteLine("This command could now be removed from your solution.");
        WriteLine("The commands [config] and [dir] could also be deleted (practical help commands only)\n\n");

        
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