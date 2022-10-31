using System.Net.Http.Json;
using PainKiller.PowerCommands.Core.Extensions;
using PainKiller.PowerCommands.NistNvdCommands.Configuration;
using PainKiller.PowerCommands.NistNvdCommands.DomainObjects;

namespace PainKiller.PowerCommands.NistNvdCommands.Commands;

[PowerCommand(description: "Search the NIST NATIONAL VULNERABILITY DATABASE",
    useAsync: false,
    flags: "",
    example: "search")]
public class SearchCommand : CommandBase<PowerCommandsConfiguration>
{
    static readonly HttpClient Client = new HttpClient();
    public SearchCommand(string identifier, PowerCommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run()
    {
        try
        {
            Client.DefaultRequestHeaders.Add("apiKey", Configuration.Secret.DecryptSecret("##NistApiKey##"));
            var response = Client.GetFromJsonAsync<Rootobject>($"https://services.nvd.nist.gov/rest/json/cves/2.0?keywordSearch={Input.SingleArgument}&resultsPerPage=20&startIndex=0").Result;
            foreach (var vulnerability in response!.vulnerabilities) Console.WriteLine($"{vulnerability.cve.id} {vulnerability.cve.cisaRequiredAction} {vulnerability.cve.descriptions.FirstOrDefault()?.value}");
        }
        catch (HttpRequestException e)
        {
            WriteLine("\nException Caught!");
            WriteLine($"Message :{e.Message}");
        }
        return CreateRunResult();
    }

    public override async Task<RunResult> RunAsync()
    {
        try
        {
            Client.DefaultRequestHeaders.Add("apiKey", Configuration.Secret.DecryptSecret("##NistApiKey##"));
            var response = await Client.GetFromJsonAsync<Rootobject>($"https://services.nvd.nist.gov/rest/json/cves/2.0?keywordSearch={Input.SingleArgument}&resultsPerPage=20&startIndex=0");
            foreach (var vulnerability in response!.vulnerabilities) Console.WriteLine($"{vulnerability.cve.id} {vulnerability.cve.cisaRequiredAction} {vulnerability.cve.descriptions.FirstOrDefault()?.value}");
        }
        catch (HttpRequestException e)
        {
            WriteLine("\nException Caught!");
            WriteLine($"Message :{e.Message}");
        }
        return CreateRunResult();
    }
}