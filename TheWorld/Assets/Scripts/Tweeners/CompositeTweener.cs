using DG.Tweening;
using UnityEngine;
using System.Collections.Generic;
//using Sirenix.OdinInspector;

public class CompositeTweener : MonoTweener
{

    enum PlayMode
    {
        InOrder,
        Simultaneous
    }

    //[SerializeField, EnumToggleButtons] PlayMode mode;
    [SerializeField] PlayMode mode;

    [SerializeField] List<MonoTweener> tweeners;

    protected override Tweener LocalPlay()
    {
        var length = 0f;
        foreach (var tweener in tweeners)
        {
            if (mode == PlayMode.Simultaneous && length < tweener.length)
            {
                length = tweener.length;
            }
            else if (mode == PlayMode.InOrder)
            {
                if (length == 0f)
                {
                    tweenIndex = 0;
                }
                length += tweener.length;
            }
        }
        // we need a Tweener to return, so create a Tweener that does nothing
        // other than kicking off the sub-Tweeners after a delay
        var dummyTweener = transform.DOMove(transform.position, length)
                            .SetDelay(delay)
                            .OnStart(() =>
                            {
                                if (mode == PlayMode.Simultaneous)
                                {
                                    foreach (var tweener in tweeners)
                                    {
                                        tweener.Play();
                                    }
                                }
                                else
                                {
                                    tweeners[0].Play(PlayNextTweenInOrder);
                                }
                            });
        return dummyTweener;
    }

    int tweenIndex = 0;
    void PlayNextTweenInOrder()
    {
        tweenIndex++;
        if (tweenIndex < tweeners.Count)
        {
            tweeners[tweenIndex].Play(PlayNextTweenInOrder);
        }
    }
}