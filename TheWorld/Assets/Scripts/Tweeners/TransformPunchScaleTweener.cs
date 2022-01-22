using UnityEngine;
using DG.Tweening;

public class TransformPunchScaleTweener : TransformTweener
{

    [SerializeField] float scalePercent;
    [SerializeField] int vibrato = 10;
    [SerializeField, Range(0f, 1f)] float elasticity = 1f;
    [SerializeField] bool resetOnComplete;

    protected override Tweener LocalPlay()
    {
        var targetScale = target.localScale * scalePercent;
        var tween = target.DOPunchScale(targetScale, duration, vibrato, elasticity);
        if (resetOnComplete)
        {
            var startingScale = target.localScale;
            tween.onComplete += () => { target.localScale = startingScale; };
            tween.onKill += () => { target.localScale = startingScale; };
        }
        return tween;
    }
}