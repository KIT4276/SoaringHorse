using UnityEngine;

[CreateAssetMenu(menuName = "Soaring Horse/Balance/Bonus Config", fileName = "BonusConfig")]
public class BonusConfig : ScriptableObject
{
    [SerializeField] private float _minBonusY = -3;
    [SerializeField] private float _maxBonusY = 3;
    [SerializeField] private float _bonusSpacing = 10;
    [Range(0f, 1f)]
    [SerializeField] private float _luckChance = 0.8f;

    public float MinBonusY { get => _minBonusY; }
    public float MaxBonusY { get => _maxBonusY; }
    public float BonusSpacing { get => _bonusSpacing; }
    public float LuckChance {  get => _luckChance; }
}
