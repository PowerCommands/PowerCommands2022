using PowerCommands.WebShared.Models;

namespace PowerCommands.WebShared.Contracts;

public interface ICommandsService
{
    Task<RunResultModel> GetRunResultModel(InputModel input);
}