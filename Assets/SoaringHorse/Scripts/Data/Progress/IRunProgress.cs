using System;

public interface IRunProgress
{
    int Score { get; }
    int Lifes { get; }
    float Speed { get; }
    float RunTime { get; }

    event Action Changed;

    void ApplyFromSave(RunSaveData data);
    void AddScore(int delta);
    void SetLifes(int value);
    void SetSpeed (float value);
    void SetRunTime(float value);
}
