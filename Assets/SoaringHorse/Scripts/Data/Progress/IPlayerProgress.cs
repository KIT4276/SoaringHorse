using System;

public interface IPlayerProgress
{
    float Score { get; }
    int Lifes { get; }

    event Action Changed;

    void ApplyFromSave(SaveData data); 
    void AddScore(float delta);
    void SetLifes(int value);
}
