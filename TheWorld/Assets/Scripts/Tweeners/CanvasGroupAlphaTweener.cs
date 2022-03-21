using DG.Tweening;
using UnityEngine;

public class CanvasGroupAlphaTweener : MonoTweener
{
    [SerializeField] protected CanvasGroup target;
    [SerializeField] protected float targetAlpha;
    [SerializeField] protected bool targetBlockRaycast;
    [SerializeField] protected bool targetInteractable;
    protected override Tweener LocalPlay()
    {
        target.blocksRaycasts = targetBlockRaycast;
        target.interactable = targetInteractable;
        return target.DOFade(targetAlpha, duration).SetEase(easing);
    }
}
