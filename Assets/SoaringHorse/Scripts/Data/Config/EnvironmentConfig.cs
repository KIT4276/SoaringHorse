using UnityEngine;

[CreateAssetMenu(menuName = "Soaring Horse/Balance/Environment Config", fileName = "EnvironmentConfig")]
public class EnvironmentConfig : ScriptableObject
{
    [SerializeField] private float _environmentMoveSpeed = 4;
    [SerializeField] private float _spawnMargin = 7f;
    [SerializeField] private float _despawnMargin = 7f;

    public float EnvironmentMoveSpeed { get => _environmentMoveSpeed; }
    public float SpawnEnvironmentMargin { get => _spawnMargin; }
    public float DespawnEnvironmentMargin { get => _despawnMargin; }
}
