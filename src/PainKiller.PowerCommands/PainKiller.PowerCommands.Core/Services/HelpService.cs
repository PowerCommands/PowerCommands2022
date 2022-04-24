﻿using PainKiller.PowerCommands.Core.Extensions;
using PainKiller.PowerCommands.Shared.Contracts;

namespace PainKiller.PowerCommands.Core.Services;

public class HelpService : IHelpService
{
    private HelpService(){}
    private static readonly Lazy<IHelpService> Lazy = new(() => new HelpService());
    public static IHelpService Service => Lazy.Value;
    public void ShowHelp(IConsoleCommand command)
    {
        var da = command.GetPowerCommandAttribute();
        Console.Clear();
        command.WriteObjectDescription("PowerCommand", command.Identifier);
        var typeDescription = command.GetType().FullName!.StartsWith("PainKiller.PowerCommands.Core.Commands") ? "Core command (reserved name, not for use as custom command name)" : "Custom command";
        command.WriteObjectDescription("Type        ", $"{ typeDescription }");
        command.WriteObjectDescription("Full name   ", $"{command.GetType().FullName}");
        Console.WriteLine($"{da.Description}");

        var args = da.Arguments.Split('|');
        var qutes = da.Qutes.Split('|');
        var examples = da.Examples.Split('|');

        command.WriteHeaderLine($"{nameof(da.Arguments)}:");
        foreach (var a in args) Console.WriteLine(a);
        command.WriteHeaderLine($"{nameof(da.Qutes)}:");
        foreach (var q in qutes) Console.WriteLine(q);
        
        if (!string.IsNullOrEmpty(da.DefaultParameter))
        {
            command.WriteHeaderLine($"{nameof(da.DefaultParameter)}:");
            Console.WriteLine(da.DefaultParameter);
            Console.WriteLine();
        }
        command.WriteHeaderLine($"{nameof(da.Examples)}:");
        foreach (var e in examples) Console.WriteLine(e);
    }
}