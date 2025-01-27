namespace $safeprojectname$.Contracts;

public interface IInfoPanelContent
{
    string GetText();
    string? ShortText { get; }
}