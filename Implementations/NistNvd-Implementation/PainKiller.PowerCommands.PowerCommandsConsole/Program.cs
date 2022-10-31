using PainKiller.PowerCommands.Core.Services;

ConsoleService.WriteHeaderLine(nameof(Program),"NistNvd Commands 1.0");
PainKiller.PowerCommands.Bootstrap.Startup.ConfigureServices().Run(args);