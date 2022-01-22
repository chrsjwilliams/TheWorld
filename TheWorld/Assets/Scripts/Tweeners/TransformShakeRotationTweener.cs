using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TransformShakeRotationTweener : TransformTweener
{


    [SerializeField] Vector3 strength;
    [SerializeField] int vibrato;
    [SerializeField, Range(0f, 180f)] float randomness;
    [SerializeField] bool fadeOut;

    Vector3 _originalRotation;
    protected override Tweener LocalPlay()
    {
        _originalRotation = target.localRotation.eulerAngles;
        return target.DOShakeRotation(duration, strength, vibrato, randomness, fadeOut);
    }

    public void ResetRotation()
    {
        target.localRotation = Quaternion.Euler(_originalRotation);
    }

}
