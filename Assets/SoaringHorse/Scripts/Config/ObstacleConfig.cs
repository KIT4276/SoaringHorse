using UnityEngine;

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
