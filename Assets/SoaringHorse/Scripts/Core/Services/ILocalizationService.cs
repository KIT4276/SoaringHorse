using System;

public interface ILocalizationService
{
    event Action<string> LanguageChanged;

    string CurrentLanguage { get; }

    void ApplySdkLanguage(string sdkLanguageCode);
    void SetLanguage(string languageCode);
}