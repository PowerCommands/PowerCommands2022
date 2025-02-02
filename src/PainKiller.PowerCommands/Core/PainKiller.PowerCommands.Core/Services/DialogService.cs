using PainKiller.PowerCommands.Security.Services;
using PainKiller.PowerCommands.Shared.Extensions;

namespace PainKiller.PowerCommands.Core.Services;
public static class DialogService
{
    public static bool YesNoDialog(string question, string yesValue = "y", string noValue = "n")
    {
        WriteHeader($"\n{question}"); ;
        Console.WriteLine($"({yesValue}/{noValue}):");

        var response = Console.ReadLine();
        return $"{response}".Trim().ToLower() == yesValue.ToLower();
    }
    public static string QuestionAnswerDialog(string question, string prompt = "")
    {
        WriteHeader($"{question}\n");
        Console.Write(string.IsNullOrEmpty(prompt) ? IPowerCommandServices.DefaultInstance?.Configuration.Prompt : prompt);
        var response = Console.ReadLine();
        return $"{response}".Trim();
    }
    public static string SecretPromptDialog(string question, int maxRetries = 3)
    {
        var retryCount = 0;
        var secret = "";
        while (retryCount < maxRetries)
        {
            WriteHeader($"\n{question} ");
            secret = PasswordPromptService.Service.ReadPassword();
            Console.WriteLine();
            Console.Write("Confirm: ".PadLeft(question.Length + 1));
            var confirm = PasswordPromptService.Service.ReadPassword();
            if (secret != confirm)
            {
                ConsoleService.Service.WriteCritical(nameof(DialogService), "\nConfirmation failure, please try again.\n");
                retryCount++;
            }
            else break;
        }

        return $"{secret}".Trim();
    }
    public static string SelectFileDialog(string startPath = "", string filter = "", bool onlyTextFiles = false)
    {
        if (string.IsNullOrEmpty(startPath)) startPath = Environment.CurrentDirectory;
        var dir = new DirectoryInfo(startPath);
        var fileInfos = dir.GetFiles(filter);
        var files = new List<string>();
        if (onlyTextFiles)
        {
            foreach (var file in fileInfos)
            {
                file.IsPlainTextFileContent();
                files.Add(file.Name);
            }
        }
        else
        {
            files.AddRange(fileInfos.Select(f => f.Name));
        }
        var selectedIndex = ListService.ListDialog("Select a file", files);
        var selectedFile = selectedIndex.Count > 0 ? selectedIndex.First().Value : "";
        return string.IsNullOrEmpty(selectedFile) ? "" : Path.Combine(startPath, selectedFile);
    }
    public static string SelectDirectoryDialog(string startPath = "", string filter = "")
    {
        if (string.IsNullOrEmpty(startPath)) startPath = Environment.CurrentDirectory;
        var dir = new DirectoryInfo(startPath);
        var directories = dir.GetDirectories(filter).Select(d => d.Name).ToList();
            
        var selectedIndex = ListService.ListDialog("Select a file", directories);
        var selectedDir = selectedIndex.Count > 0 ? selectedIndex.First().Value : "";
        return string.IsNullOrEmpty(selectedDir) ? "" : Path.Combine(startPath, selectedDir);
    }
    private static void WriteHeader(string text)
    {
        var originalColor = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write(text);
        Console.ForegroundColor = originalColor;
    }
}