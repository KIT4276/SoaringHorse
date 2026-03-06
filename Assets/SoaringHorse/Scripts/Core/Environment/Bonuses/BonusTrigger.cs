using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class BonusTrigger : MonoBehaviour
{
    [SerializeField] private Collider2D _collider;
    [SerializeField] private Bonus _bonus;

    private void Awake()
    {
        if(_collider == null)
            _collider = GetComponent<Collider2D>();
        if(_bonus == null)
            _bonus = GetComponent<Bonus>();

        _collider.isTrigger = true;  
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<HeroMove>(out var hero))
        {
            _bonus.RewardedDespawn();
        }
    }
    
}
