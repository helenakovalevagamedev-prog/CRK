using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = "ClickerAnimationConfig", menuName = "Configs/Clicker Animation Config")]
public sealed class ClickerAnimationConfig : ScriptableObject
{
    [Header("Button")]
    [SerializeField, Min(0f)] private float punchStrength = 0.15f;
    [SerializeField, Min(0.01f)] private float punchDuration = 0.25f;
    [SerializeField, Min(0)] private int punchVibrato = 8;
    [SerializeField, Range(0f, 1f)] private float punchElasticity = 0.5f;

    [Header("Flying text")]
    [SerializeField] private float flyDistance = 250f;
    [SerializeField, Min(0.01f)] private float flyDuration = 0.8f;
    [SerializeField, Min(0f)] private float flyHorizontalSpread = 60f;
    [SerializeField] private Ease flyMoveEase = Ease.OutCubic;
    [SerializeField] private Ease flyFadeEase = Ease.InQuad;

    public float PunchStrength => punchStrength;
    public float PunchDuration => punchDuration;
    public int PunchVibrato => punchVibrato;
    public float PunchElasticity => punchElasticity;

    public float FlyDistance => flyDistance;
    public float FlyDuration => flyDuration;
    public float FlyHorizontalSpread => flyHorizontalSpread;
    public Ease FlyMoveEase => flyMoveEase;
    public Ease FlyFadeEase => flyFadeEase;
}