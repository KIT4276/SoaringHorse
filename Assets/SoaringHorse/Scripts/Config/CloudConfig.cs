using UnityEngine;

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
