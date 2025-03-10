﻿namespace $safeprojectname$.Contracts
{
    public interface IDownloadWithProgressService
    {
        Task Download(string downloadUrl, string destinationFilePath, Func<long?, long, double?, string, string, bool> progressChanged);
    }
}