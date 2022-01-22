using System;
using System.Collections.Generic;
using UnityEngine;
//using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "New Simple Event"
                , menuName = "Events/Simple")]
public class SimpleEvent : ScriptableObject
{

    List<Action> callbacks = new List<Action>();

    //[Button]
    public void Raise()
    {
        for (var i = callbacks.Count - 1; i >= 0; i--)
        {
            var callback = callbacks[i];
            if (callback == null)
            {
                callbacks.RemoveAt(i);
            }
            else
            {
                callback?.Invoke();
            }
        }
    }

    public void Subscribe(Action callback)
    {
        callbacks.Add(callback);
    }

    public void Unsubscribe(Action callback)
    {
        callbacks.Remove(callback);
    }
}
