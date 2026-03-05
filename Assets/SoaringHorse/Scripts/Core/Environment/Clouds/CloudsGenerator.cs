using UnityEngine;
using Zenject;

public class CloudsGenerator : BaseEnvironmentGenerator<Cloud>
{
    private Cloud.Factory _factory;

    [Inject]
    private void Construct(GameConfig gameConfig, Cloud.Factory cloudFactory)
    {
        _factory = cloudFactory;

        InitCommon(gameConfig.SpawnEnvironmentMargin, gameConfig.DespawnEnvironmentMargin);

        InitSpawnParams(
            gameConfig.MinCloudY,
            gameConfig.MaxCloudY,
            gameConfig.FixedCloudZ,
            gameConfig.CloudSpacing
        );
    }

    protected override Cloud SpawnEntry(Vector3 localPos)
    {
        Vector3 worldPos = LocalToWorld(localPos);

        Cloud cloud = _factory.Create(worldPos);
        Transform t = cloud.transform;

        t.SetParent(container, true);

        return cloud;
    }

    protected override bool IsEntryMissing(Cloud entry) => !entry;

    protected override float GetEntryWorldX(Cloud entry) =>
        entry ? entry.transform.position.x : float.NegativeInfinity;

    protected override void DespawnEntry(Cloud entry)
    {
        if (entry) entry.Despawn();
    }
}
