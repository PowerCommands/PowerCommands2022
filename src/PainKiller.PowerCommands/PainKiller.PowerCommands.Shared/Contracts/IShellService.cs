﻿namespace PainKiller.PowerCommands.Shared.Contracts;

public interface IShellService
{
    void Execute(string programName, string arguments, string workingDirectory, string fileExtension = "exe");
    void Execute(string programName, string arguments, string workingDirectory, Action<string, bool> writeFunction, string fileExtension = "exe");
}