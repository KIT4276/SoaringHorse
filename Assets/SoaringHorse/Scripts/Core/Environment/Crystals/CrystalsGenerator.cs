using UnityEngine;
using Zenject;

public class CrystalsGenerator : BaseEnvironmentGenerator<CrystalsGenerator.SpawnedPair>
{
    private Crystal.Factory _factory;

    [Inject]
    private void Construct(GameConfig gameConfig, Crystal.Factory crystalFactory)
    {
        _factory = crystalFactory;

        InitCommon(gameConfig.SpawnEnvironmentMargin, gameConfig.DespawnEnvironmentMargin);
        InitSpawnParams(
            gameConfig.MinObstY,
            gameConfig.MaxObstY,
            gameConfig.FixedObstZ,
            gameConfig.PairObstSpacing
        );
    }

    protected override SpawnedPair SpawnEntry(Vector3 localPos)
    {
        var a = SpawnCrystal(localPos, 0f);
        var b = SpawnCrystal(localPos, 180f);
        return new SpawnedPair(a, b);
    }

    private Crystal SpawnCrystal(Vector3 localPos, float zRotation)
    {
        Vector3 worldPos = LocalToWorld(localPos);

        Crystal cr = _factory.Create(worldPos);
        Transform t = cr.transform;

        t.SetParent(container, true);
        t.localRotation = Quaternion.Euler(0f, 0f, zRotation);

        return cr;
    }

    protected override bool IsEntryMissing(SpawnedPair entry) => !entry.A || !entry.B;

    protected override float GetEntryWorldX(SpawnedPair entry)
    {
        float ax = entry.A ? entry.A.transform.position.x : float.NegativeInfinity;
        float bx = entry.B ? entry.B.transform.position.x : float.NegativeInfinity;
        return Mathf.Max(ax, bx);
    }

    protected override void DespawnEntry(SpawnedPair entry)
    {
        if (entry.A) entry.A.Despawn();
        if (entry.B) entry.B.Despawn();
    }

    public struct SpawnedPair
    {
        public readonly Crystal A;
        public readonly Crystal B;

        public SpawnedPair(Crystal a, Crystal b)
        {
            A = a;
            B = b;
        }
    }
}