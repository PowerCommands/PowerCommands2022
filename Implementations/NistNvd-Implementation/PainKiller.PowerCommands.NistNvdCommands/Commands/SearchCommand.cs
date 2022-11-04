using System.Net.Http.Json;
namespace PainKiller.PowerCommands.NistNvdCommands.Commands;

[PowerCommandDesign(description: "Search the NIST national vulnerability database, requires that you have configured a secret named NistApiKey",
                       useAsync: false,
                        secrets: "NistApiKey",
                        example: "//Search the NIST database|search")]
public class SearchCommand : CommandBase<PowerCommandsConfiguration>
{
    protected readonly HttpClient Client = new();
    public SearchCommand(string identifier, PowerCommandsConfiguration configuration) : base(identifier, configuration) { }
    public override RunResult Run()
    {
        Client.DefaultRequestHeaders.Add("apiKey", Configuration.Secret!.DecryptSecret("##NistApiKey##"));
        var response = Client.GetFromJsonAsync<Rootobject>($"https://services.nvd.nist.gov/rest/json/cves/2.0?keywordSearch={Input.SingleArgument}&resultsPerPage=20&startIndex=0").Result;
        foreach (var vulnerability in response!.vulnerabilities) Console.WriteLine($"{vulnerability.cve.id} {vulnerability.cve.cisaRequiredAction} {vulnerability.cve.descriptions.FirstOrDefault()?.value}");
        return Ok();
    }
    public override async Task<RunResult> RunAsync()
    {
        Client.DefaultRequestHeaders.Add("apiKey", Configuration.Secret!.DecryptSecret("##NistApiKey##"));
        var response = await Client.GetFromJsonAsync<Rootobject>($"https://services.nvd.nist.gov/rest/json/cves/2.0?keywordSearch={Input.SingleArgument}&resultsPerPage=20&startIndex=0");
        foreach (var vulnerability in response!.vulnerabilities) Console.WriteLine($"{vulnerability.cve.id} {vulnerability.cve.cisaRequiredAction} {vulnerability.cve.descriptions.FirstOrDefault()?.value}");
        return Ok();
    }
}