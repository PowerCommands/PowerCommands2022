using $safeprojectname$.DomainObjects.Core;

namespace $safeprojectname$.Contracts;

public interface ITranslationManager
{
    ITranslations Translations { get; }
    void LoadTranslations();
    void SaveTranslations();
    void AddTranslations(List<TranslatedLabel> labels);
    void AddTranslation(string name, string translation);
    TranslatedLabel GetTranslationByName(string assetName);
    TranslatedLabel GetTranslationByDisplayName(string displayName);
}