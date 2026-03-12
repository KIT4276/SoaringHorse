using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEnvironmentGenerator<TEntry> : MonoBehaviour
{
    protected const float SpawnEpsilon = 0.05f;

    [SerializeField]protected Transform container;
    protected Camera targetCamera;

    protected float minY;
    protected float maxY;
    protected float fixedZ;

    protected float spacingX;
    protected float spawnMargin;
    protected float despawnMargin;

    protected readonly Queue<TEntry> entries = new Queue<TEntry>();

    protected float nextLocalX;
    protected bool initialized;

    protected void InitCommon(float spawnMargin, float despawnMargin)
    {
        this.spawnMargin = spawnMargin;
        this.despawnMargin = despawnMargin;
    }

    protected void InitSpawnParams(float minY, float maxY, float fixedZ, float spacingX)
    {
        this.minY = minY;
        this.maxY = maxY;
        this.fixedZ = fixedZ;
        this.spacingX = spacingX;
    }

    protected virtual void Awake()
    {
        if (!targetCamera) targetCamera = Camera.main;
        if (!container) container = transform;
    }

    protected virtual void Start()
    {
        InitializeNextLocalX();
        EnsureSpawnAhead();
    }

    protected virtual void Update()
    {
        DespawnOutOfView();
        EnsureSpawnAhead();
    }

    protected virtual Vector3 MakeLocalPos(float localX)
    {
        float y = Random.Range(minY, maxY);
        return new Vector3(localX, y, fixedZ);
    }

    protected Vector3 LocalToWorld(Vector3 localPos) => container.TransformPoint(localPos);

    private void EnsureSpawnAhead()
    {
        if (!initialized || !targetCamera || !container) return;
        if (spacingX <= 0f) return;

        GetCameraXBoundsWorld(out _, out float camRightWorld);

        float camRightLocal = camRightWorld - container.position.x;
        float minSpawnLocalX = camRightLocal + SpawnEpsilon;
        float maxSpawnLocalX = camRightLocal + spawnMargin;

        if (nextLocalX < minSpawnLocalX)
        {
            float delta = minSpawnLocalX - nextLocalX;
            int steps = Mathf.CeilToInt(delta / spacingX);
            nextLocalX += steps * spacingX;
        }

        while (nextLocalX <= maxSpawnLocalX)
        {
            var entry = SpawnEntry(MakeLocalPos(nextLocalX));
            entries.Enqueue(entry);
            nextLocalX += spacingX;
        }
    }

    private void DespawnOutOfView()
    {
        if (!initialized || !targetCamera) return;

        GetCameraXBoundsWorld(out float camLeftWorld, out _);
        float threshold = camLeftWorld - despawnMargin;

        while (entries.Count > 0)
        {
            var entry = entries.Peek();

            if (IsEntryMissing(entry))
            {
                entries.Dequeue();
                continue;
            }

            if (GetEntryWorldX(entry) >= threshold)
                break;

            entries.Dequeue();
            DespawnEntry(entry);
        }
    }

    private void InitializeNextLocalX()
    {
        GetCameraXBoundsWorld(out _, out float camRightWorld);
        float camRightLocal = camRightWorld - container.position.x;

        nextLocalX = camRightLocal + SpawnEpsilon;
        initialized = true;
    }

    protected void GetCameraXBoundsWorld(out float left, out float right)
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

    protected abstract TEntry SpawnEntry(Vector3 localPos);
    protected abstract bool IsEntryMissing(TEntry entry);
    protected abstract float GetEntryWorldX(TEntry entry);
    protected abstract void DespawnEntry(TEntry entry);
}
