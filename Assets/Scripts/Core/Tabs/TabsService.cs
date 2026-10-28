using System;

public sealed class TabsService : ITabsService
{
    public TabType Current { get; private set; }

    public event Action<TabType> onTabOpened;
    public event Action<TabType> onTabClosed;

    public void Open(TabType tab)
    {
        if (Current == tab)
        {
            return;
        }

        if (Current != TabType.None)
        {
            onTabClosed?.Invoke(Current);
        }

        Current = tab;
        onTabOpened?.Invoke(tab);
    }
}
