using UnityEngine;

[CreateAssetMenu(menuName = "Soaring Horse/Balance/Progression Config", fileName = "ProgressionConfig")]
public class ProgressionConfig : ScriptableObject
{
    [SerializeField] private float _scoreIncreasePerTick = 1;
    [SerializeField] private float _scoreTickTime = 3;

    public float ScoreIncreasePerTick { get=> _scoreIncreasePerTick; }
    public float ScoreTickTime { get=> _scoreTickTime; }
}
