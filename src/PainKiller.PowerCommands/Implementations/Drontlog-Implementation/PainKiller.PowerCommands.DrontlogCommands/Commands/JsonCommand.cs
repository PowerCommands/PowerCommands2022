using System.Text.Json;

namespace PainKiller.PowerCommands.DrontlogCommands.Commands;

public class JsonCommand : CommandBase<PowerCommandsConfiguration>
{
    public JsonCommand(string identifier, PowerCommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run()
    {
        var items = new List<string>{"	A01:2021 – Broken Access Control	",
            "	A02:2021 – Cryptographic Failures	",
            "	A3:2017-Sensitive Data Exposure	",
            "	A03:2021-Injection	",
            "	A04:2021-Insecure Design	",
            "	A4:2017-XML External Entities (XXE)	",
            "	A05:2021-Security Misconfiguration	",
            "	A06:2021-Vulnerable and Outdated Components	",
            "	A07:2021-Identification and Authentication Failures 	",
            "	A08:2021-Software and Data Integrity Failures	",
            "	A8:2017-Insecure Deserialization	",
            "	A09:2021-Security Logging and Monitoring Failures	",
            "	A10:2021-Server-Side Request Forgery	",
            "	Allowing Domains or Accounts to Expire	",
            "	Buffer Overflow	",
            "	Business logic vulnerability	",
            "	CSV Injection (Formula Injection)	",
            "	Deserialization of untrusted data	",
            "	Directory Restriction Error	",
            "	Empty String Password	",
            "	Improper Data Validation	",
            "	Information exposure through query strings	",
            "	SQL Injection	",
            "	Insecure Randomness	",
            "	Insecure Temporary File	",
            "	Insecure Third Party Domain Access	",
            "	Insecure Transport	",
            "	Least Privilege Violation	",
            "	Insufficient validation of input data	",
            "	Missing Error Handling	",
            "	Missing XML Validation	",
            "	Hardcoded Password	",
            "	Password Plaintext Storage	",
            "	Poor Logging Practice	",
            "	Unrestricted File Upload	",
            "	Unsafe use of Reflection	",
            "	Use of Obsolete Methods	",
            "	Using a broken or risky cryptographic algorithm	",
            "	XML External Entity (XXE) Processing	",
        };
        var vulnerabilities = new Vulnerabilities();
        foreach (var item in items)
        {
            var data = item.Trim();
            vulnerabilities.Data.Add(data);
            Console.WriteLine(data);
        }

        var jsonData = JsonSerializer.Serialize(vulnerabilities);
        File.WriteAllText("vulnerabilities.json", jsonData);
        return CreateRunResult();
    }
}