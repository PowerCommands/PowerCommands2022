using System.Data.SqlClient;
using Dapper;
using PainKiller.Data.SqlExtension.DomainObjects.Queries;

namespace PainKiller.PowerCommands.DrontlogCommands.Commands;

[Tags("maintenance|util")]
[PowerCommand(      description: $"Create posts stored in {nameof(WriterInput)} table, deletes already created posts", 
                    arguments:"<delete>",
                    argumentMandatory: false,
                    example: "writerinput|writerinput delete")]
public class WriterInputCommand : DapperCommandBase
{
    public WriterInputCommand(string identifier, PowerCommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run(CommandLineInput input)
    {
        if (input.SingleArgument == "delete")
        {
            Delete();
            return CreateRunResult(input);
        }
        Create();
        return CreateRunResult(input);
    }

    private void Create()
    {
        using var connection = new SqlConnection(ConnectionString);

        var selectSql = $"SELECT * FROM [{Schema}].[{nameof(WriterInput)}] WHERE [{nameof(WriterInput.InputStatus)}] = 'U'";    //U = update
        var inputs = connection.Query<WriterInput>(selectSql).ToList();
        WriteLine($"Post to create count: {inputs.Count}");
        foreach (var writerInput in inputs)
        {
            var post = new Post { PostID = Guid.NewGuid(), Caption = writerInput.Input1, Created = DateTime.Now, OwnerProviderServiceID = writerInput.CreatedBy, ProfileID = writerInput.CreatedBy, ProviderServiceID = writerInput.ProviderServiceID, ProviderSpecificID = Guid.NewGuid().ToString(), UrlToLogo = writerInput.UrlToLogo, ProviderSpecificCreated = writerInput.Created, Updated = DateTime.Now };

            var insertPostQuery = new InsertQuery<Post>(post, insertPrimaryKeyValues: true, enableTriggers: true);
            var sql = insertPostQuery.Sql(Schema);

            connection.Query(sql);

            var content = new PostContent { MainBody = writerInput.Input2, PostID = post.PostID, Tags = "" };
            var insertContentQuery = new InsertQuery<PostContent>(content, true);
            sql = insertContentQuery.Sql(Schema);
            connection.Query(sql);

            connection.Query($"UPDATE {Schema}.{nameof(WriterInput)} SET {nameof(WriterInput.InputStatus)} = 'D' WHERE [{nameof(WriterInput.WriterInputID)}] = '{writerInput.WriterInputID}'");

            WriteLine(writerInput.Input1, false);
        }
    }
    private void Delete()
    {
        using var connection = new SqlConnection(ConnectionString);
        connection.Query($"DELETE FROM {Schema}.{nameof(WriterInput)} WHERE {nameof(WriterInput.InputStatus)} = 'D'");
        WriteLine($"Processed WriterInputs deleted from table [{nameof(WriterInput)}]");
    }
}