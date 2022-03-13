using System;
using UnityEngine;
using Sirenix.OdinInspector;

public abstract class TagAction : ScriptableObject
{
    protected Action callback;
    [Button]
    public abstract void ExecuteAction();
}