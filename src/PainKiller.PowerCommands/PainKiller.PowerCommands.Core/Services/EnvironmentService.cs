using PainKiller.PowerCommands.Shared.Contracts;
namespace PainKiller.PowerCommands.Core.Services;

public class EnvironmentService : IEnvironmentService
{
    private EnvironmentService(){}

    private static readonly Lazy<IEnvironmentService> Lazy = new(() => new EnvironmentService());
    public static IEnvironmentService Service => Lazy.Value;
    public string GetEnvironmentVariable(string variableName, bool decrypt = false)
    {
        var retVal = Environment.GetEnvironmentVariable(variableName, EnvironmentVariableTarget.User) ?? "";
        if (decrypt) retVal = EncryptionService.Service.DecryptString(retVal);
        return retVal;
    }
    public void SetEnvironmentVariable(string variableName, string inputValue, bool encrypt = false)
    {
        var val = encrypt ? EncryptionService.Service.DecryptString(inputValue) : inputValue;
        Environment.SetEnvironmentVariable(variableName, val,EnvironmentVariableTarget.User);
    }
}