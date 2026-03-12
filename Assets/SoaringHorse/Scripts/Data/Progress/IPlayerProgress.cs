using System;

public interface IPlayerProgress
{
    float Exp { get; }
    float Score { get; }
    int Lifes { get; }

    event Action Changed;

    void ApplyFromSave(SaveData data); // без эвента Changed
    void AddExp(float delta);
    void AddScore(float delta);
    void SetLifes(int value);
}
