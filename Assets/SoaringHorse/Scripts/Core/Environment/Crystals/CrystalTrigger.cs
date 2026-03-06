using UnityEngine;
using Zenject;

[RequireComponent (typeof(Collider2D))]
public class CrystalTrigger : MonoBehaviour
{
    [SerializeField] private Collider2D _collider;
    [SerializeField] private Crystal _crystal;
    

    private void Awake()
    {
        if (_collider == null)
            _collider = GetComponent<Collider2D>();

        _collider.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<HeroMove>(out var hero))
        {
            _crystal.Damage();
        }
    }
}
