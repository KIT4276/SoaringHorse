using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CrystalsGenerator : MonoBehaviour
{
    private const float SpawnEpsilon = 0.05f; // чтобы спавнить гарантированно за краем кадра

    private Transform container;
    private Camera targetCamera;

    private float minY;
    private float maxY ;
    private float fixedZ;
    private float pairSpacing;
    private float spawnMargin;
    private float despawnMargin;

    private Crystal.CrystalFactory _factory;

    private readonly Queue<SpawnedPair> _pairs = new Queue<SpawnedPair>();
    private float _nextLocalX;
    private bool _initialized;

    [Inject]
    private void Construct(GameConfig gameConfig, Crystal.CrystalFactory crystalFactory)
    {
        minY = gameConfig.MinObstY;
        maxY = gameConfig.MaxObstY;
        fixedZ = gameConfig.FixedObstZ;
        pairSpacing = gameConfig.PairObstSpacing;
        spawnMargin = gameConfig.SpawnEnvironmentMargin;
        despawnMargin = gameConfig.DespawnEnvironmentMargin;

        _factory = crystalFactory;
    }

    private void Awake()
    {
        if (!targetCamera) targetCamera = Camera.main;
        if (!container) container = transform;
    }

    private void Start()
    {
        InitializeNextLocalX();
        EnsureSpawnAhead();
    }

    private void Update()
    {
        DespawnOutOfView();
        EnsureSpawnAhead();
    }

    private void EnsureSpawnAhead()
    {
        if (!_initialized || !targetCamera || !container) return;
        if (pairSpacing <= 0f) return;

        GetCameraXBoundsWorld(out _, out float camRightWorld);

        // Камера в "локальных X контейнера" (контейнер не обязан двигаться с постоянной скоростью)
        float camRightLocal = camRightWorld - container.position.x;

        float minSpawnLocalX = camRightLocal + SpawnEpsilon;     // строго за правым краем
        float maxSpawnLocalX = camRightLocal + spawnMargin;      // не дальше, чем spawnMargin

        // Если по каким-то причинам next оказался левее правого края (телепорт/рывок скорости),
        // сдвигаем его вперёд шагами pairSpacing, сохраняя кратность расстояния.
        if (_nextLocalX < minSpawnLocalX)
        {
            float delta = minSpawnLocalX - _nextLocalX;
            int steps = Mathf.CeilToInt(delta / pairSpacing);
            _nextLocalX += steps * pairSpacing;
        }

        // Спавним только внутри окна [minSpawnLocalX; maxSpawnLocalX]
        while (_nextLocalX <= maxSpawnLocalX)
        {
            SpawnAtLocalX(_nextLocalX);
            _nextLocalX += pairSpacing;
        }
    }

    private void SpawnAtLocalX(float localX)
    {
        float y = Random.Range(minY, maxY);
        Vector3 localPos = new Vector3(localX, y, fixedZ);

        var a = SpawnCrystal(localPos, 0f);
        var b = SpawnCrystal(localPos, 180f);

        _pairs.Enqueue(new SpawnedPair(a, b));
    }

    private Crystal SpawnCrystal(Vector3 localPos, float zRotation)
    {
        // В фабрику нужно передавать WORLD позицию
        Vector3 worldPos = container.TransformPoint(localPos);

        Crystal cr = _factory.Create(worldPos);
        Transform t = cr.transform;

        // Держим в контейнере (и оставляем worldPos как есть)
        t.SetParent(container, true);
        t.localRotation = Quaternion.Euler(0f, 0f, zRotation);

        return cr;
    }

    private void DespawnOutOfView()
    {
        if (!_initialized || !targetCamera) return;

        GetCameraXBoundsWorld(out float camLeftWorld, out _);

        float threshold = camLeftWorld - despawnMargin; // левее этого — обязаны деспавнить

        while (_pairs.Count > 0)
        {
            var pair = _pairs.Peek();

            if (pair.IsMissing)
            {
                _pairs.Dequeue();
                continue;
            }

            // Пара у тебя по X совпадает, берём max для надёжности
            if (pair.WorldX >= threshold)
                break;

            _pairs.Dequeue();
            Despawn(pair);
        }
    }

    private void Despawn(SpawnedPair pair)
    {
        if (pair.A) pair.A.Despawn();
        if (pair.B) pair.B.Despawn();
    }

    private void InitializeNextLocalX()
    {
        GetCameraXBoundsWorld(out _, out float camRightWorld);
        float camRightLocal = camRightWorld - container.position.x;

        // Следующий спавн — сразу за правым краем кадра (а не +spawnMargin)
        _nextLocalX = camRightLocal + SpawnEpsilon;
        _initialized = true;
    }

    private void GetCameraXBoundsWorld(out float left, out float right)
    {
        if (targetCamera.orthographic)
        {
            float halfWidth = targetCamera.orthographicSize * targetCamera.aspect;
            float cx = targetCamera.transform.position.x;
            left = cx - halfWidth;
            right = cx + halfWidth;
            return;
        }

        Vector3 planePoint = new Vector3(0f, 0f, fixedZ);
        float depth = Vector3.Dot(planePoint - targetCamera.transform.position, targetCamera.transform.forward);
        if (depth <= 0.01f) depth = targetCamera.nearClipPlane + 0.01f;

        left = targetCamera.ViewportToWorldPoint(new Vector3(0f, 0.5f, depth)).x;
        right = targetCamera.ViewportToWorldPoint(new Vector3(1f, 0.5f, depth)).x;
    }

    private struct SpawnedPair
    {
        public readonly Crystal A;
        public readonly Crystal B;

        public SpawnedPair(Crystal a, Crystal b)
        {
            A = a;
            B = b;
        }

        public bool IsMissing => !A || !B;

        public float WorldX
        {
            get
            {
                float ax = A ? A.transform.position.x : float.NegativeInfinity;
                float bx = B ? B.transform.position.x : float.NegativeInfinity;
                return Mathf.Max(ax, bx);
            }
        }
    }
}