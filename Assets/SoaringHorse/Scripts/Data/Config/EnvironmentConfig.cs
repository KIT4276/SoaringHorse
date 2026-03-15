using UnityEngine;

[CreateAssetMenu(menuName = "Soaring Horse/Balance/Environment Config", fileName = "EnvironmentConfig")]
public class EnvironmentConfig : ScriptableObject
{
    [SerializeField] private float _environmentMoveSpeed = 4;
    [SerializeField] private float _spawnMargin = 7f;
    [SerializeField] private float _despawnMargin = 7f;
    [SerializeField] private float _speedMultiplier = 0.01f;
    [SerializeField] private float _speedIncreasePerTick = 0.02f;
    [SerializeField] private float _speedTickTime = 3;

    public float EnvironmentMoveSpeed { get => _environmentMoveSpeed; }
    public float SpawnEnvironmentMargin { get => _spawnMargin; }
    public float DespawnEnvironmentMargin { get => _despawnMargin; }
    public float SpeedMultiplier { get => _speedMultiplier; }
    public float SpeedIncreasePerTick { get => _speedIncreasePerTick; }
    public float SpeedTickTime { get => _speedTickTime; }
}
