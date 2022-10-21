namespace PainKiller.PowerCommands.Shared.Contracts;

public interface IStorageService<T> where T : new()
{
    string StoreObject(T storeObject);
    string DeleteObject();
    T GetObject();
    string Backup();
}