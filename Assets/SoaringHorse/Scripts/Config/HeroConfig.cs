using UnityEngine;

[CreateAssetMenu(menuName = "Soaring Horse/Balance/Hero Config", fileName = "HeroConfig")]
public class HeroConfig : ScriptableObject
{
    [SerializeField] private float _heroUpForce = 3;
    [SerializeField] private int _maxLives = 10;
    [SerializeField] private float _invulnerabilityTime = 3;

    public float HeroUpForce { get => _heroUpForce; }
    public int MaxLives { get => _maxLives; }
    public float InvulnerabilityTime { get => _invulnerabilityTime; }
}

[CreateAssetMenu(menuName = "Soaring Horse/Balance/Environment Config", fileName = "EnvironmentConfig")]
public class EnvironmentConfig : ScriptableObject
{
    [SerializeField] private float _environmentMoveSpeed = 1;
    [SerializeField] private float _environmentMoveIncrease = 0.2f;
    [SerializeField] private float _spawnMargin = 7f;
    [SerializeField] private float _despawnMargin = 7f;

    public float EnvironmentMoveSpeed { get => _environmentMoveSpeed; }
    public float EnvironmentMoveIncrease { get => _environmentMoveIncrease; }
    public float SpawnEnvironmentMargin { get => _spawnMargin; }
    public float DespawnEnvironmentMargin { get => _despawnMargin; }
}

[CreateAssetMenu(menuName = "Soaring Horse/Balance/Obstacle Config", fileName = "ObstacleConfig")]
public class ObstacleConfig : ScriptableObject
{
    [SerializeField] private float _minObstaclesY = -2.28f;
    [SerializeField] private float _maxObstaclesY = 2.28f;
    [SerializeField] private float _fixedZ = 0f;
    [SerializeField] private float _pairSpacing = 7f;

    public float MinObstacleY { get => _minObstaclesY; }
    public float MaxObstacleY { get => _maxObstaclesY; }
    public float FixedObstZ { get => _fixedZ; }
    public float PairObstSpacing { get => _pairSpacing; }
}

[CreateAssetMenu(menuName = "Soaring Horse/Balance/Cloud Config", fileName = "CloudConfig")]

public class CloudConfig : ScriptableObject
{
    [SerializeField] private float _minCloudsY = -5f;
    [SerializeField] private float _maxCloudsY = 5f;
    [SerializeField] private float _fixedCloudZ = 0;
    [SerializeField] private float _cloudSpacing = 7;

    public float MinCloudY { get => _minCloudsY; }
    public float MaxCloudY { get => _maxCloudsY; }
    public float FixedCloudZ { get => _fixedCloudZ; }
    public float CloudSpacing { get => _cloudSpacing; }
}

[CreateAssetMenu(menuName = "Soaring Horse/Balance/Bonus Config", fileName = "BonusConfig")]
public class BonusConfig : ScriptableObject
{
    [SerializeField] private float _minBonusY = -5;
    [SerializeField] private float _maxBonusY = 5;
    [SerializeField] private float _bonusSpacing = 5;

    public float MinBonusY { get => _minBonusY; }
    public float MaxBonusY { get => _maxBonusY; }
    public float BonusSpacing { get => _bonusSpacing; }
}

[CreateAssetMenu(menuName = "Soaring Horse/Balance/Progression Config", fileName = "ProgressionConfig")]
public class ProgressionConfig : ScriptableObject
{
    [SerializeField] private float _experienceIncrease = 0.02f;
    [SerializeField] private int _timeExperienceIncrease = 3;

    public float ExperienceIncrease { get => _experienceIncrease; }
    public int TimeExperienceIncrease { get => _timeExperienceIncrease; }
}

[CreateAssetMenu(menuName = "Soaring Horse/Balance/Scene Config", fileName = "SceneConfig")]
public class SceneConfig : ScriptableObject
{
    [SerializeField] private string _gameSceneName = "GameScene";

    public string GameSceneName { get => _gameSceneName; }
}