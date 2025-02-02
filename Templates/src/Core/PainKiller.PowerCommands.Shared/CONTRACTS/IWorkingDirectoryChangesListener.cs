﻿namespace $safeprojectname$.Contracts
{
    public interface IWorkingDirectoryChangesListener
    {
        void OnWorkingDirectoryChanged(string[] files, string[] directories);
        void InitializeWorkingDirectory();
    }
}