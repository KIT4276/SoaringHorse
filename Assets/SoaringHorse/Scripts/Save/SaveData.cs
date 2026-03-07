using System;

[Serializable]
public class SaveData 
{
    public const int CurrentVersion = 1;

    public int version = CurrentVersion;
    public float exp = 0;
    public float score = 0;
    public int lifes = 3;

    public long lastWriteUnixUtc = 0;

    public void TouchNow() => lastWriteUnixUtc = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

    public void Fixup()
    {
        if (version <= 0) version = CurrentVersion;
        if (exp < 0) exp = 0;
        if (score < 0) score = 0;
        if (lifes < 0) lifes = 0;
    }
}
