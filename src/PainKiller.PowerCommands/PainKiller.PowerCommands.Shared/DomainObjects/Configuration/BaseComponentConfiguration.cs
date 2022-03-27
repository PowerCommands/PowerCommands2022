﻿namespace PainKiller.PowerCommands.Shared.DomainObjects.Configuration;

public abstract class BaseComponentConfiguration
{
    public string Component { get; set; } = "";
    public string Checksum { get; set; } = "";
    public string Name { get; set; } = "";
}