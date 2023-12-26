using System.Globalization;
using System.Text.RegularExpressions;

namespace PainKiller.PowerCommands.Shared.Extensions;

public static class StringExtensions
{
    public static string ToFirstLetterUpper(this string input)
    {
        if (input.Length < 2) return input.ToUpper();
        var firstLetter = input.Substring(0, 1).ToUpper();
        var retVal = $"{firstLetter}{input.Substring(1, input.Length - 1).ToLower()}";
        return retVal;
    }
    public static string ToOptionDescription(this string option)
    {
        var required = option.StartsWith('!') ? " (required)" : "";
        return string.IsNullOrEmpty(option) ? "" : $"--{option.Replace("!", "")}{required}";
    }
    public static string RemoveHtml(this string content) => Regex.Replace($"{content}", @"<[^>]*>", String.Empty);
    public static List<string> ExtractLinks(this string content) => new Regex("http(s)?://([\\w+?\\.\\w+])+([a-zA-Z0-9\\~\\!\\@\\#\\$\\%\\^\\&amp;\\*\\(\\)_\\-\\=\\+\\\\\\/\\?\\.\\:\\;\\'\\,]*)?", RegexOptions.IgnoreCase).Matches(content).Select(m => m.Value).ToList();
    public static bool IsUppercaseOnly(this string input) => new Regex("^[A-Z]+$").IsMatch(input);
    public static string GetDisplayFormattedFileSize(this string fileName)
    {
        if (!File.Exists(fileName)) return $"{fileName} does not exist.";

        var fileInfo = new FileInfo(fileName);
        var bytes = fileInfo.Length;

        double size;
        string unit;

        switch (bytes)
        {
            case < 1024: // Less than 1 KB
                size = bytes;
                unit = "bytes";
                return $"{size.ToString("N2", CultureInfo.CurrentCulture)} {unit}"; // No parentheses for bytes
            case < 1048576: // 1024 * 1024, less than 1 MB
                size = bytes / 1024.0;
                unit = "KB";
                break;
            case < 1073741824: // 1024 * 1024 * 1024, less than 1 GB
                size = bytes / 1048576.0; // 1024 * 1024
                unit = "MB";
                break;
            case < 1099511627776: // 1024 * 1024 * 1024 * 1024, less than 1 TB
                size = bytes / 1073741824.0; // 1024 * 1024 * 1024
                unit = "GB";
                break;
            default: // 1 TB or more
                size = bytes / 1099511627776.0; // 1024 * 1024 * 1024 * 1024
                unit = "TB";
                break;
        }

        // Round to two decimal points and format the result using the current culture
        var formattedSize = $"{Math.Round(size, 2).ToString("N2", CultureInfo.CurrentCulture)} {unit}";
        var formattedBytes = $"{bytes.ToString("N0", CultureInfo.CurrentCulture)} bytes";
    
        return $"{formattedSize} ({formattedBytes})";
    }
    public static string GetDisplayTimeSinceLastUpdate(this DateTime lastUpdated)
    {
        var timeDifference = DateTime.Now - lastUpdated;

        if (timeDifference.TotalSeconds < 60) return $"{(int)timeDifference.TotalSeconds} seconds ago";
        if (timeDifference.TotalMinutes < 60) return $"{(int)timeDifference.TotalMinutes} minutes ago";
        if (timeDifference.TotalHours < 24) return $"{(int)timeDifference.TotalHours} hours ago";
        if (timeDifference.TotalDays < 30) return $"{(int)timeDifference.TotalDays} days ago";
        if (timeDifference.TotalDays < 365)
        {
            int months = (int)(timeDifference.TotalDays / 30);
            return $"{months} months ago";
        }
        var years = (int)(timeDifference.TotalDays / 365);
        return $"{years} years ago";
    }
}