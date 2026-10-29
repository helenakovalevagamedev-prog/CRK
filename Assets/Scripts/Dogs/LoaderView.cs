using DG.Tweening;
using UnityEngine;

public sealed class LoaderView : MonoBehaviour
{
    [SerializeField, Min(0.1f)] private float rotationDuration = 1f;

    private Tween rotation;

    private void OnEnable()
    {
        rotation = transform
            .DOLocalRotate(new Vector3(0f, 0f, -360f), rotationDuration, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear)
            .SetLoops(-1)
            .SetLink(gameObject);
    }

    private void OnDisable()
    {
        rotation?.Kill();
        transform.localRotation = Quaternion.identity;
    }
}