using System.Text.Json;
using PainKiller.PowerCommands.Core.Commands;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization;

namespace PainKiller.PowerCommands.MyExampleCommands.Commands;

[PowerCommandDesign(  description: "Converting yaml format to json or xml format",
    arguments: "<filename>",
      options: "format",
  suggestions: "xml|json",
      example: "//Convert to json format|convert \"c:\\temp\\test.yaml\" json|//Convert to xml format|convert \"c:\\temp\\test.yaml\" xml")]
public class ConvertCommand(string identifier, PowerCommandsConfiguration configuration) : CdCommand(identifier, configuration)
{
    public override RunResult Run()
    {
        var yamlInput = File.ReadAllText(Input.SingleArgument);
        var format = GetOptionValue("format");
        if (format == "json")
        {
            var jsonOutput = ConvertYamlToJson(yamlInput);
            WriteLine(jsonOutput);
        }
        return Ok();
    }
    public static string ConvertYamlToJson(string yamlInput)
    {
        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();
        var yamlObject = deserializer.Deserialize<object>(yamlInput);

        var json = JsonSerializer.Serialize(yamlObject, new JsonSerializerOptions { WriteIndented = true });
        return json;
    }
}