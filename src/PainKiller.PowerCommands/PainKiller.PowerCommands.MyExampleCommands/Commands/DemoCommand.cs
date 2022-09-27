﻿using PainKiller.PowerCommands.MyExampleCommands.Configuration;

namespace PainKiller.PowerCommands.MyExampleCommands.Commands;

[Tags("demo")]
[PowerCommand(  description: "Demo command just to se that your solution is setup properly",
                example: "demo")]
public class DemoCommand : CommandBase<PowerCommandsConfiguration>
{
    public DemoCommand(string identifier, PowerCommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run(CommandLineInput input)
    {
        WriteHeadLine("Congratulations! You have setup your PowerCommands solution correctly!");
        WriteLine("This command could now be removed from your solution.");
        return CreateRunResult(input);
    }
}