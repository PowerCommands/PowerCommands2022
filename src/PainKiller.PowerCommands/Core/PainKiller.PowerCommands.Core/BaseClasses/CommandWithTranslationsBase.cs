namespace PainKiller.PowerCommands.Core.BaseClasses;

public class CommandWithTranslationsBase<TConfig>(string identifier, TConfig configuration, bool autoShowToolbar = true, ConsoleColor[]? colors = null) : CommandWithToolbarBase<TConfig>(identifier, configuration, autoShowToolbar, colors) where TConfig : new() 
{
    public override bool InitializeAndValidateInput(ICommandLineInput input, PowerCommandDesignAttribute? designAttribute = null)
    {
        TranslationManager.Instance.LoadTranslations();
        return base.InitializeAndValidateInput(input, designAttribute);
    }
    public override RunResult Run() => Ok();
    protected void AddTranslations(List<TranslatedLabel> labels) => TranslationManager.Instance.AddTranslations(labels);
    protected void AddTranslation(string name, string translation) => TranslationManager.Instance.AddTranslation(name, translation);
    protected virtual TranslatedLabel? GetTranslationByName(string translationName) => TranslationManager.Instance.GetTranslationByName(translationName);
    protected virtual TranslatedLabel? GetTranslationByDisplayName(string displayName) => TranslationManager.Instance.GetTranslationByDisplayName(displayName);
    protected void SaveTranslations() => TranslationManager.Instance.SaveTranslations();
    protected void ShowTranslation()
    {
        WriteHeadLine("Global translations:");
        foreach (var translatedLabel in TranslationManager.Instance.Translations.TranslatedLabels.OrderBy(t => t.Name).ThenBy(t => t.DisplayName)) WriteCodeExample(translatedLabel.Name.PadRight(60), translatedLabel.DisplayName);
    }
}