//using Sirenix.OdinInspector;
using DG.Tweening;
using UnityEngine;

public class TransformPunchPositionTweener : TransformTweener
{
    // Ways to extend this:
    //      Differentiate between which axis you want to punch.
    [SerializeField] bool randomPunch;
    //[SerializeField, HideIf("randomPunch", true)] Vector3 punch;
    [SerializeField] Vector3 punch;

    //[SerializeField, ShowIf("randomPunch", true), MinMaxSlider(0, 2)] Vector2 punchRange;
    [SerializeField] Vector2 punchRange;


    [SerializeField] int vibrato;
    [SerializeField] float elasticity;
    [SerializeField] bool snapping;

    Vector3 _originalPosition;
    protected override Tweener LocalPlay()
    {
        _originalPosition = target.transform.localPosition;
        if (randomDuration)
        {
            duration = Random.Range(durationRange.x, durationRange.y);
        }
        if (randomPunch)
        {
            punch = new Vector3(0, Random.Range(punchRange.x, punchRange.y), 0);
        }

        return target.DOPunchPosition(punch, duration, vibrato, elasticity, snapping);
    }

    public void ResetPosition()
    {
        target.localPosition = _originalPosition;
    }
}
