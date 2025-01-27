using System.Reflection;
using PainKiller.PowerCommands.Shared.Events;

namespace PainKiller.PowerCommands.Core.Services
{
    public class ReflectionService : IReflectionService
    {
        internal static List<PowerCommandDesignConfiguration> CommandDesignOverrides { get; private set; } = [];
        private ReflectionService() { }
        private static readonly Lazy<IReflectionService> Lazy = new(() => new ReflectionService());
        public static IReflectionService Service => Lazy.Value;

        public event EventHandler<DesignAttributeReflectedArgs>? DesignAttributeReflected;

        public List<IConsoleCommand> GetCommands<TConfiguration>(BaseComponentConfiguration pluginInfo, TConfiguration configuration) where TConfiguration : CommandsConfiguration
        {
            var currentAssembly = Assembly.Load($"{pluginInfo.Component}".Replace(".dll", ""));
            return GetCommands(currentAssembly, configuration);
        }
        private List<IConsoleCommand> GetCommands<TConfiguration>(Assembly assembly, TConfiguration configuration) where TConfiguration : CommandsConfiguration
        {
            CommandDesignOverrides = configuration.CommandDesignOverrides;
            var retVal = new List<IConsoleCommand>();

            var types = assembly.GetTypes().Where(t => t.IsClass && t.Name.EndsWith("Command") && !t.IsAbstract).ToList();
            if (types.Count == 0) return retVal;

            foreach (var commandType in types)
            {
                var constructorInfo = commandType.GetConstructors()[0];
                var name = commandType.Name.ToLower();
                Object[] args = { name.Substring(0, name.Length - 7), (constructorInfo.GetParameters()[1].ParameterType == typeof(CommandsConfiguration) ? configuration as CommandsConfiguration : configuration) };
                var command = (IConsoleCommand)Activator.CreateInstance(commandType, args)!;
                var pcAttribute = command.GetPowerCommandAttribute();
                InvokeDesignAttributeReflected(new DesignAttributeReflectedArgs(pcAttribute, command));
                AppendWorkingDirectoryListener(command);
                retVal.Add(command);
            }
            return retVal.OrderBy(c => c.Identifier).ToList();
        }
        private void AppendWorkingDirectoryListener(IConsoleCommand command)
        {
            var workingDirectoryListener = command.GetType().GetInterface(nameof(IWorkingDirectoryChangesListener));
            if (workingDirectoryListener != null)
            {
                // ReSharper disable once SuspiciousTypeConversion.Global
                var listener = (IWorkingDirectoryChangesListener)command;
                CdCommand.WorkingDirectoryChanged += listener.OnWorkingDirectoryChanged;
                listener.InitializeWorkingDirectory();
            }
        }
        public string GetVersion(Assembly assembly) => $"{assembly.GetName().Version!.Major}.{assembly.GetName().Version!.Minor}.{assembly.GetName().Version!.Build}.{assembly.GetName().Version!.Revision}";
        private void InvokeDesignAttributeReflected(DesignAttributeReflectedArgs args) => DesignAttributeReflected?.Invoke(this, args);
    }
}