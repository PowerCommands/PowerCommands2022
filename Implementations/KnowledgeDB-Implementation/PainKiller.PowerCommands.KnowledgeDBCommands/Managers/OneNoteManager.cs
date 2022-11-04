namespace PainKiller.PowerCommands.KnowledgeDBCommands.Managers;

public class OneNoteManager : IShellExecuteManager
{
    //https://support.microsoft.com/en-gb/office/command-line-switches-in-onenote-2016-34a1a0fc-891a-4660-b6ff-286ec3f1abfe
    public void Run(ShellConfigurationItem configuration, string argument) => ShellService.Service.Execute(configuration.PathToOneNote, $"/hyperlink {argument}", "");
}