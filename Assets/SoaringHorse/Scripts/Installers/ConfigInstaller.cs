using UnityEngine;
using Zenject;

public class ConfigInstaller : MonoInstaller
{
    [SerializeField] private HeroConfig _heroConfig;
    [SerializeField] private EnvironmentConfig _environmentConfig;
    [SerializeField] private ObstacleConfig _obstacleConfig;
    [SerializeField] private CloudConfig _cloudConfig;
    [SerializeField] private BonusConfig _bonusConfig;
    [SerializeField] private ProgressionConfig _progressionConfig;
    [SerializeField] private SceneConfig _sceneConfig;

    public override void InstallBindings()
    {
        Container.Bind<HeroConfig>()
            .FromInstance(_heroConfig)
            .AsSingle();

        Container.Bind<EnvironmentConfig>()
            .FromInstance(_environmentConfig)
            .AsSingle();

        Container.Bind<ObstacleConfig>()
           .FromInstance(_obstacleConfig)
           .AsSingle();

        Container.Bind<CloudConfig>()
           .FromInstance(_cloudConfig)
           .AsSingle();

        Container.Bind<BonusConfig>()
          .FromInstance(_bonusConfig)
          .AsSingle();

        Container.Bind<ProgressionConfig>()
         .FromInstance(_progressionConfig)
         .AsSingle();

        Container.Bind<SceneConfig>()
        .FromInstance(_sceneConfig)
        .AsSingle();
    }
}