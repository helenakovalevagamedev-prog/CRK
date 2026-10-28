using System;

public interface ITabsService
{
    public TabType Current { get; }
    
    public event Action<TabType> onTabOpened;
    public event Action<TabType> onTabClosed;

    public void Open(TabType tab);
}
