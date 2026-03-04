using UnityEngine;

[CreateAssetMenu(menuName = "Soaring Horse/Balance/Game Config", fileName = "GameConfig")]
public class GameConfig : ScriptableObject
{
    [Header("Hero")]
    [SerializeField] private float heroUpForce = 3;

    [Header("Environment")]
    [SerializeField] private float environmentMoveSpeed = 1;
    [SerializeField] private float environmentMoveIncrease = 0.2f;
    [SerializeField] private float spawnMargin = 7f;       // насколько дальше за правой границей камеры держать “запас”
    [SerializeField] private float despawnMargin = 7f;

    [Header("Obstacles")]
    [SerializeField] private float minY = -2.28f;
    [SerializeField] private float maxY = 2.28f;
    [SerializeField] private float fixedZ = 0f;
    [SerializeField] private float pairSpacing = 7f;       // каждая следующая пара +7 по X (в локальных координатах контейнера)

    //Hero
    public float HeroUpForce { get => heroUpForce; }

    //Environment
    public float EnvironmentMoveSpeed { get => environmentMoveSpeed; }
    public float EnvironmentMoveIncrease { get => environmentMoveIncrease; }
    public float SpawnEnvironmentMargin { get => spawnMargin; }
    public float DespawnEnvironmentMargin { get => despawnMargin; }

    //Obstacles
    public float MinObstY { get => minY; }
    public float MaxObstY { get => maxY; }
    public float FixedObstZ { get => fixedZ; }
    public float PairObstSpacing { get => pairSpacing; }
}
