using UnityEngine;
using Zenject;

public sealed class GameInstaller : MonoInstaller
{
    [SerializeField] private TabsView tabsView;
    
    [Header("Clicker")]
    [SerializeField] private ClickerConfig clickerConfig;
    [SerializeField] private ClickerView clickerView;
    [SerializeField] private FlyingText flyingTextPrefab;
    [SerializeField] private Transform flyingTextRoot;
    
    [Header("Weather")]
    [SerializeField] private WeatherConfig weatherConfig;
    [SerializeField] private WeatherView weatherView;
    
    [Header("Dogs")]
    [SerializeField] private DogsConfig dogsConfig;
    [SerializeField] private DogsView dogsView;
    [SerializeField] private BreedItemView breedItemPrefab;
    [SerializeField] private Transform breedListRoot;
    [SerializeField] private BreedPopupView breedPopupView;

    public override void InstallBindings()
    {
        Container.Bind<IHttpClient>().To<UnityHttpClient>().AsSingle();
        Container.BindInterfacesTo<RequestQueue>().AsSingle();
        
        Container.BindInterfacesTo<TabsService>().AsSingle();
        Container.Bind<TabsView>().FromInstance(tabsView).AsSingle();
        Container.BindInterfacesTo<TabsPresenter>().AsSingle();
        
        Container.BindInstance(clickerConfig);
        Container.Bind<ClickerModel>().AsSingle();
        Container.Bind<ClickerView>().FromInstance(clickerView).AsSingle();
        Container.BindInterfacesTo<ClickerPresenter>().AsSingle();
        Container.BindInterfacesTo<EnergyRegenerator>().AsSingle();
        Container.BindInterfacesAndSelfTo<AutoCollector>().AsSingle();
        Container.BindMemoryPool<FlyingText, FlyingTextPool>()
            .WithInitialSize(10)
            .FromComponentInNewPrefab(flyingTextPrefab)
            .UnderTransform(flyingTextRoot);

        Container.BindInstance(weatherConfig);
        Container.Bind<WeatherView>().FromInstance(weatherView).AsSingle();
        Container.Bind<IWeatherService>().To<WeatherService>().AsSingle();
        Container.BindInterfacesTo<WeatherPresenter>().AsSingle();
        
        Container.BindInstance(dogsConfig);
        Container.Bind<IDogsService>().To<DogsService>().AsSingle();
        Container.Bind<DogsView>().FromInstance(dogsView).AsSingle();
        Container.BindMemoryPool<BreedItemView, BreedItemPool>()
            .WithInitialSize(10)
            .FromComponentInNewPrefab(breedItemPrefab)
            .UnderTransform(breedListRoot);
        Container.BindInterfacesTo<DogsPresenter>().AsSingle();
        Container.Bind<BreedPopupView>().FromInstance(breedPopupView).AsSingle();
    }
}
