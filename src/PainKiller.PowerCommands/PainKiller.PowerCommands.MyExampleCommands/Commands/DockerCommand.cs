using PainKiller.PowerCommands.Core.Commands;

namespace PainKiller.PowerCommands.MyExampleCommands.Commands;

[PowerCommandDesign( description: "Run docker commands",
    overrideHelpOption: true,
    example: "docker run -p 9000:9000 -p 9001:9001 minio/minio server /data --console-address \":9001\"")]
public class DockerCommand : MasterCommando
{
    public DockerCommand(string identifier, PowerCommandsConfiguration configuration) : base(identifier, configuration) { }
}