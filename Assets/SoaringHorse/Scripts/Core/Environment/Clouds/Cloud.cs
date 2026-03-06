using UnityEngine;
using Zenject;

public class Cloud : BaseRecyclable
{
    protected override void Activate()
    => ActivateRandomSprite();

    public class Factory : PlaceholderFactory<Vector3, Cloud> { }

    public class Pool : MonoPoolableMemoryPool<Vector3, IMemoryPool, Cloud> { }
}
