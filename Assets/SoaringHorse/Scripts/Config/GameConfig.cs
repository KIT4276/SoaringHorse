using UnityEngine;

[CreateAssetMenu(menuName = "Soaring Horse/Balance/Game Config", fileName = "GameConfig")]
public class GameConfig : ScriptableObject
{
    [SerializeField] private string _gameSceneName = "GameScene";
    [SerializeField] private float _experienceIncrease = 0.02f;
    [SerializeField] private int _timeExperienceIncrease = 3;

    [Header("Hero")]
    [SerializeField] private float heroUpForce = 3;
    [SerializeField] private int maxLifes = 10;
    [SerializeField] private float _invulnerabilityTime = 3;

    [Header("Environment")]
    [SerializeField] private float environmentMoveSpeed = 1;
    [SerializeField] private float environmentMoveIncrease = 0.2f;
    [SerializeField] private float spawnMargin = 7f;
    [SerializeField] private float despawnMargin = 7f;

    [Header("Obstacles")]
    [SerializeField] private float minObstaclesY = -2.28f;
    [SerializeField] private float maxObstaclesY = 2.28f;
    [SerializeField] private float fixedZ = 0f;
    [SerializeField] private float pairSpacing = 7f;

    [Header("Clouds")]
    [SerializeField] private float minCloudsY = -5f;
    [SerializeField] private float maxCloudsY = 5f;
    [SerializeField] private float _fixedCloudZ = 0;
    [SerializeField] private float _cloudSpacing = 7;

    [Header("Bonuses")]
    [SerializeField] private float _minBonusY = -5;
    [SerializeField] private float _maxBonusY = 5;
    [SerializeField] private float _bonusSpacing = 5;

    public string GameSceneName { get => _gameSceneName; }
    public int MaxLifes { get => maxLifes; }
    public float ExperienceIncrease { get => _experienceIncrease; }
    public int TimeExperienceIncrease { get => _timeExperienceIncrease; }

    //Hero
    public float HeroUpForce { get => heroUpForce; }
    public float InvulnerabilityTime { get => _invulnerabilityTime; }

    //Environment
    public float EnvironmentMoveSpeed { get => environmentMoveSpeed; }
    public float EnvironmentMoveIncrease { get => environmentMoveIncrease; }
    public float SpawnEnvironmentMargin { get => spawnMargin; }
    public float DespawnEnvironmentMargin { get => despawnMargin; }

    //Obstacles
    public float MinObstY { get => minObstaclesY; }
    public float MaxObstY { get => maxObstaclesY; }
    public float FixedObstZ { get => fixedZ; }
    public float PairObstSpacing { get => pairSpacing; }

    //Clouds
    public float MinCloudY { get => minCloudsY; }
    public float MaxCloudY { get => maxCloudsY; }
    public float FixedCloudZ { get => _fixedCloudZ; }
    public float CloudSpacing { get => _cloudSpacing; }

    //Bonuses
    public float MinBonusY { get => _minBonusY; }
    public float MaxBonusY { get => _maxBonusY; }
    public float BonusSpacing { get => _bonusSpacing; }
   
}