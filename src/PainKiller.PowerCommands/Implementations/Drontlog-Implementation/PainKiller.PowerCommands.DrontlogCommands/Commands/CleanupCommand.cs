using System.Data.SqlClient;
using Dapper;

namespace PainKiller.PowerCommands.DrontlogCommands.Commands;

[Tags("maintenance|util")]
[PowerCommand(description: $"Deletet rows older then 31 days in table [{nameof(PostCache)}]")]
public class CleanupCommand : DapperCommandBase
{
    public CleanupCommand(string identifier, PowerCommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run(CommandLineInput input)
    {
        Delete();
        return CreateRunResult(input);
    }

    private void Delete()
    {
        using var connection = new SqlConnection(ConnectionString);
        connection.Query($"DELETE FROM {Schema}.{nameof(PostCache)} WHERE {nameof(PostCache.Created)} < '{DateTime.Now.AddDays(-31)}'");
        WriteLine($"Deletet rows older then 31 days in table [{nameof(PostCache)}]");
    }
}