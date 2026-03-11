using UnityEngine;

[CreateAssetMenu(menuName = "Soaring Horse/Balance/Hero Config", fileName = "HeroConfig")]
public class HeroConfig : ScriptableObject
{
    [SerializeField] private float _heroUpForce = 4;
    [SerializeField] private int _maxLives = 10;
    [SerializeField] private float _invulnerabilityTime = 3;
    [SerializeField] private float _gravity = 0.8f;

    public float HeroUpForce { get => _heroUpForce; }
    public int MaxLives { get => _maxLives; }
    public float InvulnerabilityTime { get => _invulnerabilityTime; }
    public float Gravity { get => _gravity; }
}
