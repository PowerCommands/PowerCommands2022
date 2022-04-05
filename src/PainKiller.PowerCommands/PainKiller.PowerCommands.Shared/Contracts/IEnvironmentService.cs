namespace PainKiller.PowerCommands.Shared.Contracts;

public interface IEnvironmentService
{
    string GetEnvironmentVariable(string variableName, bool decrypt = false);
    void SetEnvironmentVariable(string variableName, string inputValue, bool encrypt = false);
}