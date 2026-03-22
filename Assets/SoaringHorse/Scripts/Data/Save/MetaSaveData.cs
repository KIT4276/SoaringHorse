using System;

[Serializable]
public class MetaSaveData
{
    public const int CurrentVersion = 1;

    public int version = CurrentVersion;

    public int bestScore = 0;
    public int totalHorseshoes = 0;
    public int totalRevives = 0;
    public float bestRunTime = 0;

    public long lastWriteUnixUtc = 0;

    public void TouchNow() => lastWriteUnixUtc = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

    public void Fixup()
    {
        if (version <= 0) version = CurrentVersion;

        if (bestScore < 0) bestScore = 0;
        if (totalHorseshoes < 0) totalHorseshoes = 0;
        if (totalRevives < 0) totalRevives = 0;
        if (bestRunTime < 0) bestRunTime = 0;
    }
}
