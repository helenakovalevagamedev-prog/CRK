using System;
using UnityEngine;

public sealed class TabsView : MonoBehaviour
{
    [SerializeField] private TabEntry[] tabs;
    [SerializeField] private Color activeColor = new (0.25f, 0.55f, 1f);
    [SerializeField] private Color inactiveColor = new (0.2f, 0.2f, 0.2f);

    public event Action<TabType> onTabClicked;

    private void Awake()
    {
        foreach (var tab in tabs)
        {
            var type = tab.Type;
            tab.Button.onClick.AddListener(() => onTabClicked?.Invoke(type));
        }
    }

    private void OnDestroy()
    {
        foreach (var tab in tabs)
        {
            tab.Button.onClick.RemoveAllListeners();
        }
    }

    public void Show(TabType active)
    {
        foreach (var tab in tabs)
        {
            bool isActive = tab.Type == active;
            tab.Content.SetActive(isActive);

            if (tab.Background != null)
            {
                tab.Background.color = isActive ? activeColor : inactiveColor;
            }
        }
    }
}
