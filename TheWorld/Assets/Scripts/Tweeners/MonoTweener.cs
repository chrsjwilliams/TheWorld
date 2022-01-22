using System;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using DG.Tweening;

public abstract class MonoTweener : MonoBehaviour
{

    [InfoBox("$description")]
    enum Editable
    {
        Locked,
        EditDescription
    }
    [SerializeField, HideLabel, EnumToggleButtons] Editable canEditDescription = Editable.Locked;

    [SerializeField, TextArea, HideLabel, ShowIf("canEditDescription", Editable.EditDescription)] string description;


    [SerializeField] protected bool completeIfKilled;
    [SerializeField] protected bool randomDuration;
    [SerializeField, HideIf("randomDuration", true)] protected float duration;

    [SerializeField, ShowIf("randomDuration", true), MinMaxSlider(0, 5)] protected Vector2 durationRange;

    [SerializeField] protected bool randomDelay;
    [SerializeField, HideIf("randomDelay", true)] protected float delay;

    [SerializeField, ShowIf("randomDelay", true), MinMaxSlider(0, 1)] protected Vector2 delayRange;

    [SerializeField] protected bool playWhilePaused;
    [SerializeField] protected bool loop;
    [InfoBox("Setting loops to -1 will make the tween loop infinitely.")]
    [SerializeField, ShowIf("loop")] protected int loops;

    [SerializeField, ShowIf("loop")] protected LoopType loopType;

    [SerializeField, FoldoutGroup("Events")] UnityEvent OnBegin;

    [SerializeField, FoldoutGroup("Events")] UnityEvent OnComplete;


    [SerializeField, FoldoutGroup("Events")] UnityEvent OnKill;


    public float length { get { return duration + delay; } }

    private Tweener tweener;

    private bool isActive, complete;

    [Button]
    public void Play()
    {
        Play(() => { });
    }

    public void Kill()
    {
        if (tweener != null)
        {
            tweener.Kill(completeIfKilled);
        }
    }

    public void Play(Action callback)
    {
        if (isActive)
        {
            return;
        }
        complete = false;
        isActive = true;
        tweener = SetupPlay();
        tweener.onComplete += () =>
        {
            complete = true;
            OnComplete?.Invoke();
            callback();
            Cleanup();
        };
        tweener.onKill += () =>
        {
            if (!complete && !completeIfKilled)
            {
                OnKill?.Invoke();
                callback();
                Cleanup();
            }
        };

    }

    void Cleanup()
    {
        isActive = false;
        tweener = null;
    }

    Tweener SetupPlay()
    {
        OnBegin?.Invoke();
        tweener = LocalPlay();
        if (delay > 0f)
        {
            tweener.SetDelay(delay);
        }
        if (loop)
        {
            tweener.SetLoops(loops, loopType);
        }
        tweener.SetUpdate(playWhilePaused);
        return tweener;
    }

    void OnDestroy()
    {
        Kill();
        Cleanup();
    }

    protected abstract DG.Tweening.Tweener LocalPlay();

}
