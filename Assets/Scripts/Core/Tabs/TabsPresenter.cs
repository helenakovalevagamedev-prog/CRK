using System;
using Zenject;

public sealed class TabsPresenter : IInitializable, IDisposable
{
    private readonly TabsView view;
    private readonly ITabsService service;

    public TabsPresenter(TabsView view, ITabsService service)
    {
        this.view = view;
        this.service = service;
    }

    public void Initialize()
    {
        view.onTabClicked += service.Open;
        service.onTabOpened += view.Show;

        service.Open(TabType.Clicker);
    }

    public void Dispose()
    {
        view.onTabClicked -= service.Open;
        service.onTabOpened -= view.Show;
    }
}
