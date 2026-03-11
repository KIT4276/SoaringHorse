using UnityEngine;
using Zenject;

public class BootstrapUIInstaller : MonoInstaller
{
    [SerializeField] private StartMenu _startMenuPrefab;

    public override void InstallBindings()
    {
        Container.Bind<StartMenuController>()
            .AsSingle();

        Container.Bind<StartMenu>()
            .FromComponentInNewPrefab(_startMenuPrefab)
            .AsSingle()
            .NonLazy();
    }
}
