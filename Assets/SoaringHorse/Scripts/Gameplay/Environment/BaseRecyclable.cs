using UnityEngine;
using Zenject;

public abstract class BaseRecyclable : MonoBehaviour, IPoolable<Vector3, IMemoryPool>
{
    [SerializeField] protected GameObject[] _images;

    protected IMemoryPool _pool;

    protected abstract void Activate();

    public void OnSpawned(Vector3 spawnPos, IMemoryPool pool)
    {
        _pool = pool;
        transform.position = spawnPos;

        gameObject.SetActive(true);

        Activate();
    }


    public void Despawn()
    {
        _pool?.Despawn(this);
    }

    public virtual void OnDespawned()
    {
        _pool = null;
        gameObject.SetActive(false);
    }

    protected void ActivateRandomSprite()
    {
        var i = Random.Range(0, _images.Length);

        for (int j = 0; j < _images.Length; j++)
        {
            if (j == i)
                _images[j].gameObject.SetActive(true);
            else
                _images[j].gameObject.SetActive(false);
        }
    }
}
