using UnityEngine;
using Zenject;

public class EnvironmentInstaller : MonoInstaller
{
    [SerializeField] private Crystal _crystalPrefab;
    [SerializeField] private Cloud _cloudPrefab;
    [SerializeField] private Bonus _bonusPrefab;
    [SerializeField] private int _initialSize = 20;
    [Space]
    [SerializeField] private GameUI _uiPrefab;

    private const string EnvGroup = "EnvironmentPoolRoot";

    public override void InstallBindings()
    {
        InstallCrystals();
        InstallClouds();
        InstallBonuses();

        BindUI();
}

    private void BindUI()
    {
        Container.BindInterfacesAndSelfTo<GameUI>()
            .FromComponentInNewPrefab(_uiPrefab)
            .AsSingle();
    }

    private void InstallCrystals()
    {
        Container
            .BindFactory<Vector3, Crystal, Crystal.Factory>()
            .FromPoolableMemoryPool<Vector3, Crystal, Crystal.Pool>(pool => pool
                .WithInitialSize(_initialSize)
                .FromComponentInNewPrefab(_crystalPrefab)
                .UnderTransformGroup(EnvGroup));
    }

    private void InstallClouds()
    {
        Container
            .BindFactory<Vector3, Cloud, Cloud.Factory>()
            .FromPoolableMemoryPool<Vector3, Cloud, Cloud.Pool>(pool => pool
                .WithInitialSize(_initialSize)
                .FromComponentInNewPrefab(_cloudPrefab)
                .UnderTransformGroup(EnvGroup));
    }

    private void InstallBonuses()
    {
        Container
            .BindFactory<Vector3, Bonus, Bonus.Factory>()
            .FromPoolableMemoryPool<Vector3, Bonus, Bonus.Pool>(pool => pool
                .WithInitialSize(_initialSize)
                .FromComponentInNewPrefab(_bonusPrefab)
                .UnderTransformGroup(EnvGroup));
    }
}
