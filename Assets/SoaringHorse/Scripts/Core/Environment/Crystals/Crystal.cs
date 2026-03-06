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

    protected override void Activate()
    {
        ActivateRandomSprite();
        _dirty = false;
    }

    public  void Damage()
    {
        if (!_dirty)
        {
            _liveSystem.SubtractLives(_value);
            _dirty = true;
        }
    }

    public class Factory : PlaceholderFactory<Vector3, Crystal> { }

    public class Pool : MonoPoolableMemoryPool<Vector3, IMemoryPool, Crystal> { }
}
