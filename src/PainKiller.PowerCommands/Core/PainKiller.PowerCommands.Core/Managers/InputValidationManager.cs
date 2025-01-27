namespace PainKiller.PowerCommands.Core.Managers
{
    public class InputValidationManager(IConsoleCommand command, ICommandLineInput input)
    {
        public InputValidationResult ValidateAndInitialize()
        {
            var retVal = new InputValidationResult();
            var messages = new List<string>();
            var attribute = command.GetPowerCommandAttribute();
            var requiredArguments = attribute.Arguments.Split(ConfigurationGlobals.ArraySplitter).Where(a => a.StartsWith('!')).ToArray();
            if (input.Arguments.Length < requiredArguments.Length)
            {
                messages.Add($"Missing argument(s), required arguments is {requiredArguments.Length}");
                retVal.HasValidationError = true;
            }
            var requiredQuotes = attribute.Quotes.Split(ConfigurationGlobals.ArraySplitter).Where(q => q.StartsWith('!')).ToArray();
            if (input.Quotes.Length < requiredQuotes.Length)
            {
                messages.Add($"Missing quote(s), required quotes is {requiredQuotes.Length}");
                retVal.HasValidationError = true;
            }

            var optionInfos = attribute.Options.Split(ConfigurationGlobals.ArraySplitter).Select(f => new PowerOption(f)).ToList();
            retVal.Options.AddRange(optionInfos);

            foreach (var optionInfo in optionInfos)
            {
                var hasOption = input.HasOption(optionInfo.Name);
                var optionValue = input.GetOptionValue(optionInfo.Name);
                if (optionInfo.IsMandatory && !hasOption)
                {
                    retVal.HasValidationError = true;
                    messages.Add($"Option [{optionInfo.Name}] is mandatory.");
                    continue;
                }
                if (optionInfo.ValueIsRequired && (string.IsNullOrEmpty(optionValue) && hasOption))
                {
                    retVal.HasValidationError = true;
                    messages.Add($"Option [{optionInfo.Name}] is required to have a value.");
                    continue;
                }
                if (!hasOption) continue;
                optionInfo.Value = optionValue;
            }

            var requiredSecrets = attribute.Secrets.Split(ConfigurationGlobals.ArraySplitter).Where(s => s.StartsWith('!')).ToArray();
            foreach (var secretName in requiredSecrets)
            {
                var secret = IPowerCommandServices.DefaultInstance!.Configuration.Secret.Secrets.FirstOrDefault(s => s.Name.ToLower() == secretName.Replace("!", "").ToLower());
                if (secret != null) continue;
                messages.Add($"Secret [{secretName.Replace("!", "")}] is required");
                retVal.HasValidationError = true;
            }

            foreach (var message in messages) ConsoleService.Service.WriteError(nameof(InputValidationManager), message);
            return retVal;
        }
    }
}