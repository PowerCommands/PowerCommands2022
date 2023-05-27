using PainKiller.PowerCommands.Core.Services;

ConsoleService.Service.WriteLine(nameof(Program),@"______                        _____                                           _     
| ___ \                      /  __ \                                         | |    
| |_/ /____      _____ _ __  | /  \/ ___  _ __ ___  _ __ ___   __ _ _ __   __| |___ 
|  __/ _ \ \ /\ / / _ \ '__| | |    / _ \| '_ ` _ \| '_ ` _ \ / _` | '_ \ / _` / __|
| | | (_) \ V  V /  __/ |    | \__/\ (_) | | | | | | | | | | | (_| | | | | (_| \__ \
\_|  \___/ \_/\_/ \___|_|     \____/\___/|_| |_| |_|_| |_| |_|\__,_|_| |_|\__,_|___/", ConsoleColor.Cyan);
ConsoleService.Service.WriteHeaderLine(nameof(Program),"\nVersion 1.0");
PainKiller.PowerCommands.Bootstrap.Startup.ConfigureServices().Run(args);

