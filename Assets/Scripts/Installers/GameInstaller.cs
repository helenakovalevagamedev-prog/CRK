using UnityEngine;
using Zenject;

public sealed class GameInstaller : MonoInstaller
{
    [SerializeField] private TabsView tabsView;
    [Header("Clicker")]
    [SerializeField] private ClickerConfig clickerConfig;
    [SerializeField] private ClickerView clickerView;

    public override void InstallBindings()
    {
        
        
        Container.BindInterfacesTo<TabsService>().AsSingle();
        Container.Bind<TabsView>().FromInstance(tabsView).AsSingle();
        Container.BindInterfacesTo<TabsPresenter>().AsSingle();
        
        Container.BindInstance(clickerConfig);
        Container.Bind<ClickerModel>().AsSingle();
        Container.Bind<ClickerView>().FromInstance(clickerView).AsSingle();
        Container.BindInterfacesTo<ClickerPresenter>().AsSingle();
        Container.BindInterfacesTo<EnergyRegenerator>().AsSingle();
        Container.BindInterfacesAndSelfTo<AutoCollector>().AsSingle();
    }
}
