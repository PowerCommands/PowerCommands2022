namespace PainKiller.PowerCommands.Shared.Contracts
{
    public interface IEncryptionService
    {
        string EncryptString(string plainText);
        string DecryptString(string plainText);
    }
}