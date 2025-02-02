using $safeprojectname$.Contracts;

namespace $safeprojectname$.DomainObjects.Core;
public class Translations : ITranslations
{
    public List<TranslatedLabel> TranslatedLabels { get; set; } = [];
}