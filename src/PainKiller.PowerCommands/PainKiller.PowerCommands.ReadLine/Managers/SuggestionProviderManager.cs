namespace PainKiller.PowerCommands.ReadLine.Managers;

public class SuggestionProviderManager
{
    public Func<string, string[]> SuggestionProviderFunc;
    public SuggestionProviderManager()
    {
        SuggestionProviderFunc = GetSuggestions;
    }
    private static string[] GetSuggestions(string input)
    {
        try
        {
            var inputs = input.Split(" ").ToList();
            if (inputs.Count < 2) return null!;

            inputs.RemoveAt(0);

            var buildPath = new List<string>();
            var startOfPathNotFound = true;
            foreach (var inputFragment in inputs)
            {
                if (inputFragment.Contains(":\\")) startOfPathNotFound = false;
                if (!startOfPathNotFound) buildPath.Add(inputFragment);
            }
            var filePath = string.Join(" ", buildPath).Replace("\"", "");
            if (!Directory.Exists(filePath)) return null!;

            var directoryInfo = new DirectoryInfo(filePath);
            var filter = directoryInfo.GetDirectories().Select(f => f.Name.Substring(0, f.Name.Length)).ToList();
            var fileFilter = directoryInfo.GetFiles().Select(f => f.Name.Substring(0, f.Name.Length)).ToArray();
            filter.AddRange(fileFilter);
            var retVal = filter.ToArray();
            return retVal;
        }
        catch (Exception e)
        {
            return new[] { e.Message };
        }
    }
}