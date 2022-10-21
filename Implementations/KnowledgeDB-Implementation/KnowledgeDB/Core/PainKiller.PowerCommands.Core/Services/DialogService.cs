﻿namespace PainKiller.PowerCommands.Core.Services;

public static class DialogService
{
    public static bool YesNoDialog(string question, string yesValue = "y", string noValue = "n")
    {
        Console.WriteLine($"{question} ({yesValue}/{noValue}):");
        var response = Console.ReadLine();
        return $"{response}".Trim().ToLower() == yesValue.ToLower();
    }
}