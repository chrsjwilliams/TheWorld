using UnityEngine;
using DG.Tweening;

public class TransformScaleTweener : TransformTweener
{
    [SerializeField] Vector3 scale;

    Vector3 _originalScale;
    protected override Tweener LocalPlay()
    {
        _originalScale = target.localScale;
        return target.DOScale(scale, duration).SetEase(easing);
    }

    public void ResetScale()
    {
        target.localScale = _originalScale;
    }

    public void SetToTargetScale()
    {
        target.localScale = scale;
    }

    public void ScaleToOne()
    {
        target.localScale = Vector3.one;
    }
}
