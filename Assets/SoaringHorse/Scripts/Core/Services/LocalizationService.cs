using System;
using System.Collections.Generic;
using UnityEngine;

public class LocalizationService: ILocalizationService
{
    public event Action<string> LanguageChanged;

    public string CurrentLanguage { get; private set; } = DefaultLanguage;

    private const string DefaultLanguage = "en";

    private readonly HashSet<string> _supportedLanguages = new()
    {
        "ru",
        "en"
        // добавл€й сюда "tr", "de" и т.д., если по€в€тс€
    };

    public void ApplySdkLanguage(string sdkLanguageCode)
    {
        string mappedLanguage = MapSdkLanguage(sdkLanguageCode);
        SetLanguage(mappedLanguage);
    }

    public void SetLanguage(string languageCode)
    {
        string normalized = Normalize(languageCode);

        if (!_supportedLanguages.Contains(normalized))
            normalized = DefaultLanguage;

        if (CurrentLanguage == normalized)
            return;

        CurrentLanguage = normalized;
        LanguageChanged?.Invoke(CurrentLanguage);
        Debug.Log("[LocalizationService] " + CurrentLanguage);
    }

    private string MapSdkLanguage(string sdkLanguageCode)
    {
        string normalized = Normalize(sdkLanguageCode);

        if (_supportedLanguages.Contains(normalized))
            return normalized;

        return DefaultLanguage;
    }

    private static string Normalize(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
            return DefaultLanguage;

        return code.Trim().ToLowerInvariant();
    }
}
