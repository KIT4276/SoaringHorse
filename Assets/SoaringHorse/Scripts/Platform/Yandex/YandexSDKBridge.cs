using UnityEngine;
using System.Runtime.InteropServices;

public static class YandexSDKBridge
{
#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")] private static extern void YG_Init(string goName);
    [DllImport("__Internal")] private static extern void YG_Ready();
    [DllImport("__Internal")] private static extern void YG_ShowFullscreenAdv();
    [DllImport("__Internal")] private static extern void YG_ShowRewardedVideo();
    [DllImport("__Internal")] private static extern void YG_PlayerGetData();
    [DllImport("__Internal")] private static extern void YG_PlayerSetData(string json);

    public static void Init(string goName) => YG_Init(goName);
    public static void Ready() => YG_Ready();
    public static void ShowInterstitial() => YG_ShowFullscreenAdv();
    public static void ShowRewarded() => YG_ShowRewardedVideo();
    public static void CloudLoad() => YG_PlayerGetData();
    public static void CloudSave(string json) => YG_PlayerSetData(json);
#else
    public static void Init(string goName) { }
    public static void Ready() { }
    public static void ShowInterstitial() { }
    public static void ShowRewarded() { }
    public static void CloudLoad() { }
    public static void CloudSave(string json) { }
#endif
}
