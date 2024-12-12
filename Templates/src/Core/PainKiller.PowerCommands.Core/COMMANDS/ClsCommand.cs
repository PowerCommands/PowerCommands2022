namespace $safeprojectname$.Commands;

[PowerCommandTest(        tests: " ")]
[PowerCommandDesign(description: "Clears the console",
             disableProxyOutput: true)]
public class ClsCommand(string identifier, CommandsConfiguration configuration) : CommandBase<CommandsConfiguration>(identifier, configuration)
{
    public override RunResult Run()
    {
        Console.Clear();
        Console.WriteLine("\x1b[3J");   //This magic ANSI sequence tells the console to clear the whole buffer and scrollbars, it needs a Console.Clear before and after, just in case, should work on most operating systems.
        Console.Clear();
        return Ok();
    }
}