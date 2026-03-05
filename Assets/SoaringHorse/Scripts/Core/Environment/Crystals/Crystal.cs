using UnityEngine;
using Zenject;

public class Crystal : BaseRecyclable
{
    protected override void ActivateSprite() => 
        ActivateRandomSprite();

    public class Factory : PlaceholderFactory<Vector3, Crystal> { }

    public class Pool : MonoPoolableMemoryPool<Vector3, IMemoryPool, Crystal> { }
}
