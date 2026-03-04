using UnityEngine;
using Zenject;

public class GameplayInstaller : MonoInstaller
{
    [SerializeField] private GameConfig _gameConfig;
    [SerializeField] private Crystal _crystalPrefab;
    [SerializeField] private int _initialSize = 20;

    private const string Env = "Environment";

    public override void InstallBindings()
    {
        InstallConfig();
        InstallCrystals();

        //InstallInput(); TODO

    }

    private void InstallInput()
    {
        Container.Bind<InputManager>()
            .AsSingle()
            .NonLazy();
    }

    private void InstallConfig()
    {
        Container.Bind<GameConfig>()
            .FromInstance(_gameConfig)
            .AsSingle();
    }

    private void InstallCrystals()
    {
        Container
            .BindFactory<Vector3, Crystal, Crystal.CrystalFactory>()
            .FromPoolableMemoryPool<Vector3, Crystal, Crystal.CrystalPool>(poolBinder => poolBinder
            .WithInitialSize(_initialSize)
            .FromComponentInNewPrefab(_crystalPrefab)
            .UnderTransformGroup(Env));
    }
}