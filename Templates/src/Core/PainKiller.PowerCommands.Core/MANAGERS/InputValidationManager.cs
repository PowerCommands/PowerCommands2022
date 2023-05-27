﻿namespace $safeprojectname$.Managers;
public class InputValidationManager
{
    private readonly ICommandLineInput _input;
    private readonly IConsoleCommand _command;
    private readonly Action<string> _logger;
    public InputValidationManager(IConsoleCommand command, ICommandLineInput input, Action<string> logger)
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
            _logger.Invoke($"Missing argument(s), required arguments is {requiredArguments.Length}");
            retVal.HasValidationError = true;
        }
        var requiredQuotes = attribute.Quotes.Split('|').Where(q => q.StartsWith('!')).ToArray();
        if (_input.Quotes.Length < requiredQuotes.Length)
        {
            _logger.Invoke($"Missing quote(s), required quotes is {requiredQuotes.Length}");
            retVal.HasValidationError = true;
        }

        var optionInfos = attribute.Options.Split('|').Select(f => new PowerOption(f)).ToList();
        retVal.Options.AddRange(optionInfos);
        
        foreach (var optionInfo in optionInfos.Where(f => f.ValueIsRequired || f.IsMandatory))
        {
            if (optionInfo.IsMandatory && !_input.HasOption(optionInfo.Name))
            {
                retVal.HasValidationError = true;
                _logger.Invoke($"Option [{optionInfo.Name}] is mandatory");
            }
            optionInfo.Value = _input.GetOptionValue(optionInfo.Name);
            if (!string.IsNullOrEmpty(optionInfo.Value) || !_input.HasOption(optionInfo.Name) || !optionInfo.ValueIsRequired) continue;
            _logger.Invoke($"Option [{optionInfo.Name}] is required to have a value or not used at all.");
            retVal.HasValidationError = true;
        }

        if (string.IsNullOrEmpty(attribute.Secrets)) return retVal;
        var requiredSecrets = attribute.Secrets.Split('|').Where(s => s.StartsWith('!')).ToArray();
        foreach (var secretName in requiredSecrets)
        {
            var secret = IPowerCommandServices.DefaultInstance!.Configuration.Secret.Secrets.FirstOrDefault(s => s.Name.ToLower() == secretName.Replace("!","").ToLower());
            if (secret != null) continue;
            _logger.Invoke($"Secret [{secretName.Replace("!","")}] is required");
            retVal.HasValidationError = true;
        }
        return retVal;
    }
}