using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;

[System.Serializable]
public class DialogLine: IAction
{
    
    public CharacterData speaker;
    [TextArea]
    public string line;
    [SerializeField] List<TagAction> lineActions = new List<TagAction>();

    [Space(50)]
    [SerializeField, TextArea]
    string Notes;

    public void AddTagAction(TagAction action)
    {
        lineActions.Add(action);
    }

    public void ExecuteTagActions(Action callback)
    {
        foreach(TagAction lineAction in lineActions)
        {
            lineAction.ExecuteAction();
        }

        callback?.Invoke();
    }

    
}