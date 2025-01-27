namespace $safeprojectname$.Contracts
{
    public interface IEncryptionService
    {
        string EncryptString(string plainText);
        string DecryptString(string plainText);
    }
}