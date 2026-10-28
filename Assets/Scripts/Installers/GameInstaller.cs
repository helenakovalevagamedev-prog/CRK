using UnityEngine;
using Zenject;

public sealed class GameInstaller : MonoInstaller
{
    [SerializeField] private TabsView tabsView;

    public override void InstallBindings()
    {
        Container.BindInterfacesTo<TabsService>().AsSingle();
        Container.Bind<TabsView>().FromInstance(tabsView).AsSingle();
        Container.BindInterfacesTo<TabsPresenter>().AsSingle();
    }
}
