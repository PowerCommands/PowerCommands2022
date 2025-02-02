using PainKiller.PowerCommands.Shared.Contracts;

namespace PainKiller.PowerCommands.Shared.DomainObjects.Core;
public class Translations : ITranslations
{
    public List<TranslatedLabel> TranslatedLabels { get; set; } = [];
}