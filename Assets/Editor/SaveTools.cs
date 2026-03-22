#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public static class SaveTools
{
    private const string LocalKeyPrefix = "save_v";

    [MenuItem("Tools/Save/Clear All Saves")]
    public static void ClearAllSaves()
    {
        var key = $"{LocalKeyPrefix}{RunSaveData.CurrentVersion}";

        var hasKey = PlayerPrefs.HasKey(key);

        PlayerPrefs.DeleteKey(key);
        PlayerPrefs.Save();

        Debug.Log(hasKey
            ? $"Save deleted. Key: {key}"
            : $"Save not found, but delete was attempted. Key: {key}");
    }

    [MenuItem("Tools/Save/Clear All PlayerPrefs")]
    public static void ClearAllPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        Debug.Log("All PlayerPrefs cleared.");
    }
}
#endif