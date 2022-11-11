namespace PainKiller.PowerCommands.Core.Managers;
public class InputValidationManager
{
    private readonly ICommandLineInput _input;
    private readonly IConsoleCommand _command;
    private readonly Action<string, bool> _logger;
    public bool DisplayAndWriteToLog = true;
    public InputValidationManager(IConsoleCommand command, ICommandLineInput input, Action<string, bool> logger)
    {
        _input = input;
        _command = command;
        _logger = logger;
    }
    public InputValidationResult ValidateAndInitialize()
    {
        var retVal = new InputValidationResult();
        var attribute = _command.GetPowerCommandAttribute();
        var requiredArguments = attribute.Arguments.Split('|').Where(a => a.StartsWith('!')).ToArray();
        if (_input.Arguments.Length < requiredArguments.Length)
        {
            _logger.Invoke($"Missing argument(s), required arguments is {requiredArguments.Length}", DisplayAndWriteToLog);
            retVal.HasValidationError = true;
        }
        var requiredQuotes = attribute.Quotes.Split('|').Where(q => q.StartsWith('!')).ToArray();
        if (_input.Quotes.Length < requiredQuotes.Length)
        {
            _logger.Invoke($"Missing quote(s), required quotes is {requiredQuotes.Length}", DisplayAndWriteToLog);
            retVal.HasValidationError = true;
        }

        var flagInfos = attribute.Flags.Split('|').Select(f => new PowerFlag(f)).ToList();
        retVal.Flags.AddRange(flagInfos);
        
        foreach (var flagInfo in flagInfos.Where(f => f.IsRequired))
        {
            flagInfo.Value = _input.GetFlagValue(flagInfo.Name);

            if (!string.IsNullOrEmpty(flagInfo.Value) || !_input.HasFlag(flagInfo.Name)) continue;
            _logger.Invoke($"Flag [{flagInfo.Name}] is required to have a value or not used at all.", DisplayAndWriteToLog);
            retVal.HasValidationError = true;
        }

        if (string.IsNullOrEmpty(attribute.Secrets)) return retVal;
        var requiredSecrets = attribute.Secrets.Split('|').Where(s => s.StartsWith('!')).ToArray();
        foreach (var secretName in requiredSecrets)
        {
            if (IPowerCommandServices.DefaultInstance!.Configuration.Secret.Secrets == null)
            {
                _logger.Invoke($"Secret [{secretName}] is required", DisplayAndWriteToLog);
                retVal.HasValidationError = true;
                break;
            }
            var secret = IPowerCommandServices.DefaultInstance!.Configuration.Secret.Secrets.FirstOrDefault(s => s.Name.ToLower() == secretName.Replace("!","").ToLower());
            if (secret == null)
            {
                _logger.Invoke($"Secret [{secretName.Replace("!","")}] is required", DisplayAndWriteToLog);
                retVal.HasValidationError = true;
            }
        }
        return retVal;
    }
}