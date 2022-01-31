using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;

[System.Serializable]
public class DialogLine
{
    
    public CharacterData speaker;
    [TextArea]
    public string line;
    List<TagAction> lineActions = new List<TagAction>();

    [Space(50)]
    [SerializeField, TextArea]
    string Notes;

    public void AddTagAction(TagAction action)
    {
        lineActions.Add(action);
    }

    public void ExecuteLineActions(Action callback)
    {
        foreach(TagAction lineAction in lineActions)
        {
            lineAction.ExecuteAction();
        }

        callback?.Invoke();
    }

    
}