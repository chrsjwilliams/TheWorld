using UnityEngine;
using System.Collections.Generic;
//using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "Tweener Collection"
                , menuName = "Collection/Tweener Collection")]
public class TweenersCollectionSO : ScriptableObject
{
    public List<MonoTweener> contents = new List<MonoTweener>();

    public int Count { get { return contents.Count; } }

    public void Add(MonoTweener monoTweener)
    {
        contents.Add(monoTweener);
    }

    public void Remove(MonoTweener monoTweener)
    {
        contents.Remove(monoTweener);
    }

}