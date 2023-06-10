using PainKiller.PowerCommands.Core.Services;
ConsoleService.Service.WriteLine(nameof(Program),@" _   _____________   __   _____ 
| | / /  _  \ ___ \ /  | |  _  |
| |/ /| | | | |_/ / `| | | |/' |
|    \| | | | ___ \  | | |  /| |
| |\  \ |/ /| |_/ / _| |_\ |_/ /
\_| \_/___/ \____/  \___(_)___/ ", ConsoleColor.Cyan);
ConsoleService.Service.WriteHeaderLine(nameof(Program), "\nKnowledgeDB Commands 1.0");
PainKiller.PowerCommands.Bootstrap.Startup.ConfigureServices().Run(args);