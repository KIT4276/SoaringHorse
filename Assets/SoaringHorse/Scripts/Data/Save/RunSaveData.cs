using System;
using UnityEngine;

[Serializable]
public class RunSaveData 
{
    public const int CurrentVersion = 1;

    public int version = CurrentVersion;

    public float score = 0;
    public int lifes = 3;
    public float speed = 4;
    public float runTime = 0;

    public long lastWriteUnixUtc = 0;

    public void TouchNow() => lastWriteUnixUtc = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

    public void Fixup()
    {
        if (version <= 0) version = CurrentVersion;
        if (score < 0) score = 0;
        if (lifes < 0) lifes = 0;
        if(speed < 4) speed = 4;
        if(runTime < 0) runTime = 0;
    }
}
