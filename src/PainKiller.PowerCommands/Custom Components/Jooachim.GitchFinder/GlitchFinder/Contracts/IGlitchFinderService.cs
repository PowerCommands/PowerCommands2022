using GlitchFinder.Managers;

namespace GlitchFinder.Contracts;

public interface IGlitchFinderService
{
    bool Run(CommandLineArgumentManager command);
}