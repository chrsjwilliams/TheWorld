using UnityEngine;
using DG.Tweening;
//using Sirenix.OdinInspector;

public class RectTransformAnchoredPositionTweener : RectTransformTweener
{

    enum AdjustBy
    {
        Offset,
        Position
    }

    //[SerializeField, EnumToggleButtons] AdjustBy tweenBy;
    [SerializeField] AdjustBy tweenBy;


    //[PropertyTooltip("Tween to this position exactly")]
    //[SerializeField, ShowIf("tweenBy", AdjustBy.Position)]
    [SerializeField]
    Vector2 position;

    //[PropertyTooltip("Tween to the offset relative to the position it started from")]
    //[SerializeField, ShowIf("tweenBy", AdjustBy.Offset)]
    [SerializeField]
    Vector2 offset;
    [SerializeField] Ease easing;

    protected override Tweener LocalPlay()
    {
        if(randomDelay)
        {
            delay = Random.Range(delayRange.x, delayRange.y);
        }
        var targetPosition = tweenBy == AdjustBy.Offset ?
                                target.anchoredPosition + offset
                                : position;
        return target.DOAnchorPos(targetPosition, duration).SetEase(easing).SetDelay(delay);
    }

    public void SetToTargetPosition()
    {
        if(tweenBy == AdjustBy.Position)
        {
            target.anchoredPosition = position;
        }
        else
        {
            target.anchoredPosition = new Vector2(  target.anchoredPosition.x + offset.x,
                                                    target.anchoredPosition.y + offset.y); 
        }
    }
}
