using System.Data.SqlClient;
using Dapper;

namespace PainKiller.PowerCommands.DrontlogCommands.Commands;

[Tags("download|checksum|example")]
[PowerCommand(description: "Download logos from ProviderServiceReplica table", useAsync: true)]
public class LogosCommand : DapperCommandBase
{
    public LogosCommand(string identifier, PowerCommandsConfiguration configuration) : base(identifier, configuration) { }
    public override async Task<RunResult> RunAsync(CommandLineInput input)
    {
        var logos = GetUrlToLogo();
        await DownloadLogos(logos);
        WriteLine("All logos downloaded");
        Console.Write($"\n{ConfigurationConstants.Prompt}");
        return CreateRunResult(input);
    }
    private List<ProviderServiceReplica> GetUrlToLogo()
    {
        using var connection = new SqlConnection(ConnectionString);
        var retVal = connection.Query<ProviderServiceReplica>($"SELECT {nameof(ProviderServiceReplica.ProviderServiceID)}, {nameof(ProviderServiceReplica.UrlToLogo)} FROM {Schema}.{nameof(ProviderServiceReplica)}");
        return retVal.ToList();
    }

    private async Task DownloadLogos(List<ProviderServiceReplica> logos)
    {
        foreach (var logo in logos)
        {
            var fileName = Path.Combine("logos", logo.UrlToLogo.Split('/').Last());
            var buffer = new byte[8192];
            var isMoreToRead = true;

            using var httpClient = new HttpClient { Timeout = TimeSpan.FromDays(1) };
            await using var response = await httpClient.GetStreamAsync(logo.UrlToLogo);
            await using var fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None, 8192, true);
            do
            {
                var bytesRead = await response.ReadAsync(buffer);
                if (bytesRead == 0)
                {
                    isMoreToRead = false;
                    continue;
                }
                await fileStream.WriteAsync(buffer.AsMemory(0, bytesRead));
            }
            while (isMoreToRead);
            WriteLine($"{logo.UrlToLogo} downloaded");
        }
    }
}