using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public sealed class TabEntry
{
    [SerializeField] private TabType type;
    [SerializeField] private Button button;
    [SerializeField] private Image background;
    [SerializeField] private GameObject content;
    
    public TabType Type => type;
    public Button Button => button;
    public Image Background => background;
    public GameObject Content => content;
}
