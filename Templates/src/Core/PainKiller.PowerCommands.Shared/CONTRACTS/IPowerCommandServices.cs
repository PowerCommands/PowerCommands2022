﻿using Microsoft.Extensions.Logging;

namespace $safeprojectname$.Contracts
{
    public interface IPowerCommandServices
    {
        IPowerCommandsRuntime Runtime { get; }
        ICommandsConfiguration Configuration { get; }
        ILogger Logger { get; }
        IDiagnosticManager Diagnostic { get; }
        IConsoleService DefaultConsoleService { get; }
        IInfoPanelManager InfoPanelManager { get; }
        public static IPowerCommandServices? DefaultInstance { get; protected set; }
    }
}