using System;

public interface IPlayerProgress
{
    int Exp { get; }
    int Score { get; }
    int Lifes { get; }

    event Action Changed;

    void ApplyFromSave(SaveData data); // без эвента Changed
    void AddExp(int delta);
    void AddScore(int delta);
    void SetLifes(int value);
}
