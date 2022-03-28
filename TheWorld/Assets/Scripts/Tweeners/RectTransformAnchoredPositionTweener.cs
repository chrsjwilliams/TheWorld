using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

public class RectTransformAnchoredPositionTweener : RectTransformTweener
{

    enum AdjustBy
    {
        Offset,
        Position,
    }

    [SerializeField, EnumToggleButtons] AdjustBy tweenBy;
    [SerializeField, ShowIf("tweenBy", AdjustBy.Position)] bool varyTargetPosition;
    [PropertyTooltip("Tween to this position exactly")]
    [SerializeField, ShowIf("tweenBy", AdjustBy.Position), HideIf("varyTargetPosition")]
    Vector2 position;
    [SerializeField, ShowIf("tweenBy", AdjustBy.Position), ShowIf("varyTargetPosition")]
    IntVariable x;
    [SerializeField, ShowIf("tweenBy", AdjustBy.Position), ShowIf("varyTargetPosition")]
    IntVariable y;

    [PropertyTooltip("Tween to the offset relative to the position it started from")]
    [SerializeField, ShowIf("tweenBy", AdjustBy.Offset)]
    Vector2 offset;

    protected override Tweener LocalPlay()
    {
        if (target == null)
        {
            return null;
        }
        if (randomDelay)
        {
            delay = Random.Range(delayRange.x, delayRange.y);
        }

        Vector2 newPos;
        if (varyTargetPosition)
        {
            if (y == null)
            {
                newPos = new Vector2(x.value, target.anchoredPosition.y);
            }
            else if (x == null)
            {
                newPos = new Vector2(target.anchoredPosition.x, y.value);

            }
            else
            {
                newPos = new Vector2(x.value, y.value);
            }


        }
        else
        {
            newPos = position;
        }
        var targetPosition = tweenBy == AdjustBy.Offset ?
                                target.anchoredPosition + offset
                                : newPos;
        return target.DOAnchorPos(targetPosition, duration).SetEase(easing).SetDelay(delay);
    }

    public void SetToTargetPosition()
    {
        if (tweenBy == AdjustBy.Position)
        {
            target.anchoredPosition = position;
        }
        else
        {
            target.anchoredPosition = new Vector2(target.anchoredPosition.x + offset.x,
                                                    target.anchoredPosition.y + offset.y);
        }
    }
}
