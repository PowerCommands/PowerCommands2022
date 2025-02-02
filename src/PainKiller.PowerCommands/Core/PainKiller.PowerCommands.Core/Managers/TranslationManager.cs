using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization;

namespace PainKiller.PowerCommands.Core.Managers;

public class TranslationManager : ITranslationManager
{
    private static TranslationManager? _instance;
    private TranslationManager() => Translations = new Translations();
    public static TranslationManager Instance => _instance ??= new TranslationManager();

    public ITranslations Translations { get; private set; }

    public void LoadTranslations()
    {
        var filePath = Translations.GetGlobalTranslationsFileName();
        if (!File.Exists(filePath)) return;
        var yaml = File.ReadAllText(filePath);
        Translations = yaml.GetObjectFromYaml<Translations>();
    }
    public void SaveTranslations()
    {
        var yaml = GetYaml();
        File.WriteAllText(Translations.GetGlobalTranslationsFileName(), yaml);
    }
    public void AddTranslations(List<TranslatedLabel> labels) => Translations.TranslatedLabels.AddRange(labels);
    public void AddTranslation(string name, string translation)
    {
        var existing = Translations.TranslatedLabels.FirstOrDefault(t => t.Name == name);
        if (existing != null) Translations.TranslatedLabels.Remove(existing);
        Translations.TranslatedLabels.Add(new TranslatedLabel
        {
            DisplayName = string.IsNullOrEmpty(translation) ? name : translation,
            Name = name
        });
    }
    public TranslatedLabel GetTranslationByName(string assetName) => Translations.TranslatedLabels.FirstOrDefault(t => t.Name == assetName) ?? new TranslatedLabel { DisplayName = assetName, Name = assetName };
    public TranslatedLabel GetTranslationByDisplayName(string displayName) => Translations.TranslatedLabels.FirstOrDefault(t => t.DisplayName == displayName) ?? new TranslatedLabel { DisplayName = displayName, Name = displayName };
    private string GetYaml()
    {
        var serializer = new SerializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();
        return serializer.Serialize(Translations);
    }
}