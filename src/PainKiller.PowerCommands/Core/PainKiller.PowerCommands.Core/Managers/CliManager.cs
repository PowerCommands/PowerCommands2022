﻿using PainKiller.PowerCommands.Configuration.DomainObjects;
using PainKiller.PowerCommands.Core.Services;
using PainKiller.PowerCommands.Shared.Contracts;

namespace PainKiller.PowerCommands.Core.Managers;

public class CliManager : ICliManager
{
    private readonly string _name;
    private readonly string _path;
    private readonly string _srcCodeRootPath;
    private readonly Action<string, bool> _logger;
    
    public bool DisplayAndWriteToLog = true;

    public CliManager(string name, string path, Action<string, bool> logger)
    {
        _name = name;
        _path = path;
        _srcCodeRootPath = Path.Combine(ConfigurationGlobals.ApplicationDataFolder, "download", name);
        _logger = logger;
    }
    public void CreateRootDirectory(bool onlyRepoSrcCodeRootPath = false)
    {
        if (!onlyRepoSrcCodeRootPath)
        {
            var dirI = new DirectoryInfo(_path);
            Directory.CreateDirectory(dirI.FullName);
            _logger.Invoke($"Directory {dirI.Attributes} created", DisplayAndWriteToLog);
        }

        Directory.CreateDirectory(_srcCodeRootPath);
        _logger.Invoke($"Directory {_srcCodeRootPath} created", DisplayAndWriteToLog);
    }

    public void DeleteDownloadsDirectory()
    {
        if(!Directory.Exists(_srcCodeRootPath)) return;

        var dirInfo = new DirectoryInfo(Path.Combine(_srcCodeRootPath, "PowerCommands2022\\.git\\objects\\pack"));
        foreach (var fileSystemInfo in dirInfo.GetFileSystemInfos())
            File.SetAttributes(fileSystemInfo.FullName, FileAttributes.Normal);
        foreach (var file in dirInfo.GetFiles())
        {
            File.Delete(file.FullName);
        }
        DeleteDir(_srcCodeRootPath);
    }

    public void CreateDirectory(string name)
    {
        var dirI = new DirectoryInfo(Path.Combine(_path, name));
        Directory.CreateDirectory(dirI.FullName);
        _logger.Invoke($"Directory {dirI.Attributes} created", DisplayAndWriteToLog);
    }
    public void CloneRepo(string repo)
    {
        ShellService.Service.Execute("git", $"clone {repo}", _srcCodeRootPath, _logger, waitForExit: true);
    }

    public void DeleteDir(string directory)
    {
        var dirPath = GetPath(directory);
        _logger($"Delete directory {dirPath}", DisplayAndWriteToLog);
        if(Directory.Exists(dirPath)) Directory.Delete(dirPath, recursive: true);
    }

    public void DeleteFile(string fileName, bool repoFile)
    {
        var path = repoFile ? Path.Combine(_srcCodeRootPath, fileName) : Path.Combine(_path, fileName);
        _logger($"Delete file {path}", DisplayAndWriteToLog);
        if(File.Exists(path)) File.Delete(path);
    }

    public void RenameDirectory(string directory, string name)
    {
        var oldDirName = directory.Split('\\').Last();
        var newDirName = directory.Replace($"\\{oldDirName}", $"\\{name}");

        var oldDirPath = GetPath(directory);
        var newDirPath = GetPath(newDirName);
        Directory.Move(oldDirPath, newDirPath);
        
        _logger.Invoke("", false);
        _logger.Invoke($"Directory moved from [{directory}]", DisplayAndWriteToLog);
        _logger.Invoke($"to [{newDirPath}]", DisplayAndWriteToLog);
        _logger.Invoke("", false);
    }
    public void MoveFile(string fileName, string toFileName)
    {
        var oldFilePath = GetPath(fileName);
        var newFilePath = GetPath(toFileName);

        File.Move(oldFilePath, newFilePath);
        _logger.Invoke("", false);
        _logger.Invoke($"File moved from [{oldFilePath}]", DisplayAndWriteToLog);
        _logger.Invoke($"to [{newFilePath}]", DisplayAndWriteToLog);
        _logger.Invoke("", false);
    }

    public void MoveDirectory(string dirctoryName, string toDirctoryName)
    {
        var oldFilePath = GetPath(dirctoryName);
        var newFilePath = GetPath(toDirctoryName);
        
        Directory.Move(oldFilePath, newFilePath);
        _logger.Invoke("", false);
        _logger.Invoke($"Directory moved from [{oldFilePath}]", DisplayAndWriteToLog);
        _logger.Invoke($"to [{newFilePath}]", DisplayAndWriteToLog);
        _logger.Invoke("", false);
    }

    public string BackupDirectory(string dirctoryName)
    {
        var backupRoot = Path.Combine(_srcCodeRootPath, "backup");
        if (!Directory.Exists(backupRoot)) Directory.CreateDirectory(backupRoot);
        
        var fullPathSource = Path.Combine(_path, dirctoryName);
        var fullPathTarget = backupRoot;

        CopyFolder(fullPathSource, fullPathTarget);

        _logger.Invoke("", false);
        _logger.Invoke($"Directory [{fullPathSource}]", DisplayAndWriteToLog);
        _logger.Invoke($"Backed up to [{fullPathTarget}]", DisplayAndWriteToLog);
        _logger.Invoke("", false);
        return backupRoot;
    }

    public void WriteNewSolutionFile()
    {
        var solutionFile = Path.Combine(_srcCodeRootPath, "PowerCommands2022\\src\\PainKiller.PowerCommands\\PainKiller.PowerCommands.sln");
        var contentRows = File.ReadAllLines(solutionFile);
        var validProjectFiles = new[]
        {
            "PainKiller.PowerCommands.Bootstrap\\PainKiller.PowerCommands.Bootstrap.csproj",
            "PainKiller.PowerCommands.PowerCommandsConsole\\PainKiller.PowerCommands.PowerCommandsConsole.csproj",
            "Core",
            "Third party components",
            "Third party components\\PainKiller.SerilogExtensions\\PainKiller.SerilogExtensions.csproj",
            "Core\\PainKiller.PowerCommands.Configuration\\PainKiller.PowerCommands.Configuration.csproj",
            "Core\\PainKiller.PowerCommands.Core\\PainKiller.PowerCommands.Core.csproj",
            "Core\\PainKiller.PowerCommands.ReadLine\\PainKiller.PowerCommands.ReadLine.csproj",
            "Core\\PainKiller.PowerCommands.Security\\PainKiller.PowerCommands.Security.csproj",
            "Core\\PainKiller.PowerCommands.Shared\\PainKiller.PowerCommands.Shared.csproj",
            "PainKiller.PowerCommands.MyExampleCommands\\PainKiller.PowerCommands.MyExampleCommands.csproj"
        };

        var validProjectsRows = new List<string>();
        foreach (var row in contentRows)
        {
            if(row.Trim().StartsWith("EndProject")) continue;
            if (row.Trim().StartsWith("Project("))
            {
                var vssProjectRef = new VSSolutionProjectReference(row);
                if (validProjectFiles.All(p => p != vssProjectRef.ProjectFilePath)) continue;
                if (vssProjectRef.ProjectFilePath == "PainKiller.PowerCommands.MyExampleCommands\\PainKiller.PowerCommands.MyExampleCommands.csproj")
                {
                    var newProjectRow = row.Replace("MyExample", _name);
                    validProjectsRows.Add($"{newProjectRow}\nEndProject\n");
                    continue;
                }
                validProjectsRows.Add($"{row}\nEndProject\n");
                continue;
            }
            validProjectsRows.Add(row);
        }
        var solutionFileName = Path.Combine(_srcCodeRootPath, $"PowerCommands2022\\src\\PainKiller.PowerCommands\\PowerCommands.{_name}.sln");
        File.WriteAllLines(solutionFileName, validProjectsRows);
        _logger.Invoke($"New solution file [{solutionFileName}] created", DisplayAndWriteToLog);
    }

    public void ReplaceContentInFile(string fileName, string find, string replace)
    {
        var filePath = GetPath(fileName);
        var content = File.ReadAllText(filePath);
        content = content.Replace(find, replace);
        File.WriteAllText(filePath, content);
        _logger.Invoke($"Content replaced in file [{fileName}]", DisplayAndWriteToLog);
    }

    public static string GetLocalSolutionRoot()
    {
        var parts = AppContext.BaseDirectory.Split('\\');
        var endToRemove = $"\\{parts[^5]}\\{parts[^4]}\\{parts[^3]}\\{parts[^2]}";
        return AppContext.BaseDirectory.Replace(endToRemove, "");
    }
    private string GetPath(string path) => path.StartsWith("PowerCommands2022\\") ? Path.Combine(_srcCodeRootPath, path) : Path.Combine(_path, path);

    private void CopyFolder(string sourceFolder, string destFolder)
    {
        if (!Directory.Exists(destFolder))
            Directory.CreateDirectory(destFolder);
        var files = Directory.GetFiles(sourceFolder);
        foreach (var file in files)
        {
            var name = Path.GetFileName(file);
            var dest = Path.Combine(destFolder, name);
            File.Copy(file, dest);
        }
        var folders = Directory.GetDirectories(sourceFolder);
        foreach (var folder in folders)
        {
            var name = Path.GetFileName(folder);
            var dest = Path.Combine(destFolder, name);
            CopyFolder(folder, dest);
        }
    }
}