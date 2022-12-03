namespace PainKiller.PowerCommands.MyExampleCommands.Commands;

[PowerCommandDesign( description: "Demo command just to se that your solution is setup properly",
                         options: "diagnostic-only|!hello-world",
                         example: "demo")]
public class DemoCommand : CommandBase<PowerCommandsConfiguration>
{
    public DemoCommand(string identifier, PowerCommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run()
    {
        WriteLine("Hello World!");
        WriteHeadLine("Congratulations! You have setup your PowerCommands solution correctly!");
        WriteLine("You may set up some things in your PowerCommandsConfiguration.yaml file, like the path to your favorite code editor");
        WriteLine("Just type dir and hit enter to open up this applications bin directory");
        WriteLine("");
        WriteLine("This command could now be removed from your solution.");
        WriteLine("The commands [config] and [dir] could also be deleted (practical help commands only)\n\n");

        
        WriteHeadLine("Diagnostic output of the command line input, for testing purposes");
        WriteLine($"Raw input: {Input.Raw}n");
        WriteLine($"Arguments: {string.Join(' ', Input.Arguments)}");
        WriteLine($"SingleArgument: {string.Join(' ', Input.SingleArgument)}\n");
        WriteLine($"Quotes: {string.Join(' ', Input.Quotes)}");
        WriteLine($"SingleQuote: {string.Join(' ', Input.SingleQuote)}\n");
        WriteHeadLine("Declared Options, with value if any.");
        foreach (var powerOption in Options)
        {
            WriteLine($"Raw input: {powerOption.Raw}");
            WriteLine($"{powerOption.Name} {powerOption.Value} isRequired: {powerOption.IsRequired}\n");
        }
        WriteHeadLine("Input Options, with value if any.");
        foreach (var powerOption in Input.Options) WriteLine($"Raw input: {powerOption} Value: {GetOptionValue(powerOption.Replace("--",""))}\n");
        return Ok();
    }
}