using $safeprojectname$.DomainObjects.Core;

namespace $safeprojectname$.Contracts;
public interface ITranslations
{
    List<TranslatedLabel> TranslatedLabels { get; set; }
}