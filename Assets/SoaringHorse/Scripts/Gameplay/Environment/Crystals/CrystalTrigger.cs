using UnityEngine;
using Zenject;

[RequireComponent (typeof(Collider2D))]
public class CrystalTrigger : MonoBehaviour
{
    [SerializeField] private Collider2D _collider;
    [SerializeField] private Crystal _crystal;
    [SerializeField] private ParticleSystem _effect;

    private void Awake()
    {
        if (_collider == null)
            _collider = GetComponent<Collider2D>();

        _collider.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<HeroAnimator>(out var hero))
        {
            if (_crystal.TryDamage())
            {
                Vector2 hitPoint = _collider.ClosestPoint(collision.transform.position);
                hero.PlayDamage(hitPoint);
            }
        }
    }
}
