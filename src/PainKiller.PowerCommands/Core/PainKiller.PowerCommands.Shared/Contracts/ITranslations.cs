using PainKiller.PowerCommands.Shared.DomainObjects.Core;

namespace PainKiller.PowerCommands.Shared.Contracts;
public interface ITranslations
{
    List<TranslatedLabel> TranslatedLabels { get; set; }
}