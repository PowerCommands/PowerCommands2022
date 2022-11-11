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
}