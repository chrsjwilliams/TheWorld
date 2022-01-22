using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

public class TransformRotateTweener : TransformTweener
{
    [EnumToggleButtons] public RotationMode rotationMode;

    [SerializeField] Vector3 degrees;
    [SerializeField] protected Ease easing;
    [SerializeField] protected bool snapping;
    

    protected override Tweener LocalPlay()
    {
        var targetRot = Vector3.zero;
        if (rotationMode == RotationMode.AddDegrees)
        {
            targetRot = target.rotation.eulerAngles;
            targetRot += degrees;
        }
        else
        {
            targetRot = degrees;
        }
        return target.DOLocalRotate(targetRot, duration, RotateMode.FastBeyond360).SetEase(easing);
    }


    public enum RotationMode
    {
        AddDegrees, RotateTo
    }
}
