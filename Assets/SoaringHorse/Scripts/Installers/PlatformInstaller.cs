using UnityEngine;
using Zenject;

public class PlatformInstaller : MonoInstaller
{
    [SerializeField] private YandexPlatform _platformPrefab;
    [SerializeField] private CoroutineRunner _coroutineRunnerPrefab;
    public override void InstallBindings()
    {
        
        
        Container.Bind<YandexPlatform>()
        .FromComponentInNewPrefab(_platformPrefab)
        .AsSingle()
        .OnInstantiated((InjectContext ctx, YandexPlatform p) =>
        {
            p.gameObject.name = "YandexPlatform";
        })
        .NonLazy();

        Container.Bind<CoroutineRunner>()
            .FromComponentInNewPrefab(_coroutineRunnerPrefab)
            .AsSingle()
            .OnInstantiated((InjectContext ctx, CoroutineRunner r) =>
            {
                r.gameObject.name = "CoroutineRunner";
            })
            .NonLazy();

        Container.Bind<MonoBehaviour>()
            .WithId("CoroutineRunner")
            .To<CoroutineRunner>()
            .FromResolve();
    }
}
