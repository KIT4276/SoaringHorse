using UnityEngine;
using Zenject;

public class Crystal : MonoBehaviour, IPoolable<Vector3, IMemoryPool>
{
    [SerializeField] private GameObject[] _images;
    
    private IMemoryPool _pool;

    public void OnSpawned(Vector3 spawnPos, IMemoryPool pool)
    {
        _pool = pool;
        transform.position = spawnPos;

        gameObject.SetActive(true);

        ActivateRandomSprite();
    }

    private void ActivateRandomSprite()
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

    public void OnDespawned()
    {
        _pool = null;
        gameObject.SetActive(false);
    }

    // Вызывать это при подборе/уничтожении кристалла
    public void Despawn()
    {
        _pool?.Despawn(this);
    }


    public class CrystalFactory : PlaceholderFactory<Vector3, Crystal> { }

    public class CrystalPool : MonoPoolableMemoryPool<Vector3, IMemoryPool, Crystal> { }
}
