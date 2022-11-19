using Microsoft.AspNetCore.Mvc;
using PainKiller.PowerCommands.Core.Extensions;
using PainKiller.PowerCommands.Shared.Contracts;
using PainKiller.PowerCommands.Shared.DomainObjects.Core;
using PowerCommands.WebShared.Models;

namespace PowerCommands.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PowerCommandController : ControllerBase
{
    [HttpPost]
    public ActionResult<RunResultModel> Post(InputModel inputModel)
    {
        var commands = IPowerCommandsRuntime.DefaultInstance?.Commands ?? new List<IConsoleCommand>();
        var input = inputModel.Raw.Interpret();

        var command = commands.FirstOrDefault(c => c.Identifier == input.Identifier);
        if (command != null)
        {
            var response = IPowerCommandServices.DefaultInstance?.Runtime.ExecuteCommand(inputModel.Raw);
            if (response != null) return new ActionResult<RunResultModel>(new RunResultModel{Input = response.Input as CommandLineInput ?? new CommandLineInput() ,Output = response.Output,Status = response.Status});
            return new ActionResult<RunResultModel>(new RunResultModel());
        }
        return new ActionResult<RunResultModel>(new RunResultModel());
    }

    [HttpGet("metadata")]
    public ActionResult<List<CommandMetadata>> GetMetadata()
    {
        var retVal = new List<CommandMetadata>();
        var commands = IPowerCommandsRuntime.DefaultInstance?.Commands ?? new List<IConsoleCommand>();
        foreach (var command in commands)
        {
            var pcAttrib = command.GetPowerCommandAttribute();
            var metadata = new CommandMetadata { Identifier = command.Identifier };
            if (!string.IsNullOrEmpty(pcAttrib.Arguments)) metadata.Parameters.AddRange(pcAttrib.Arguments.Split('|'));
            if (!string.IsNullOrEmpty(pcAttrib.Quotes)) metadata.Parameters.AddRange(pcAttrib.Quotes.Split('|'));
            if (!string.IsNullOrEmpty(pcAttrib.Options)) metadata.Options.AddRange(pcAttrib.Options.Split('|').Select(f => new PowerOption(f)));
            if (!string.IsNullOrEmpty(pcAttrib.Examples)) metadata.Examples.AddRange(pcAttrib.Examples.Split('|'));
            retVal.Add(metadata);
        }
        return new ActionResult<List<CommandMetadata>>(retVal);
    }
}