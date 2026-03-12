using UnityEngine;
using Zenject;

public class Crystal : BaseRecyclable
{
    [Space]
    [SerializeField] private int _value = 1;

    private LiveSystem _liveSystem;
    private bool _dirty = false;

    [Inject]
    private void Construct(LiveSystem liveSystem)
        => _liveSystem = liveSystem;

    public bool TryDamage()
    {
        if (!_dirty)
        {
            _liveSystem.SubtractLives(_value);
            _dirty = true;
            return true;
        }
        else
            return false;
    }

    protected override void Activate()
    {
        ActivateRandomSprite();
        _dirty = false;
    }

    public class Factory : PlaceholderFactory<Vector3, Crystal> { }

    public class Pool : MonoPoolableMemoryPool<Vector3, IMemoryPool, Crystal> { }
}
