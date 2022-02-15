using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "New Dialog Node"
                , menuName = "Dialog/Node")]
[System.Serializable]
public class DialogNode : SerializedScriptableObject, IAction
{
    [SerializeField] public string NodeTitle;
    [SerializeField] public List<DialogLine> speakingLines = new List<DialogLine>();
    [SerializeField] public Dictionary<PersonalityChoice, DialogNode> nextNodes = new Dictionary<PersonalityChoice, DialogNode>();

    [TextArea]
    [SerializeField] public string Notes;

    [SerializeField] List<TagAction> lineActions = new List<TagAction>();

    public void AddTagAction(TagAction action)
    {
        lineActions.Add(action);
    }

    public void ExecuteTagActions(Action callback)
    {
        foreach (TagAction lineAction in lineActions)
        {
            lineAction.ExecuteAction();
        }

        callback?.Invoke();
    }

    public bool HasPersonalityChoice(PersonalityChoice choice)
    {
        return nextNodes.ContainsKey(choice);
    }

}

public enum PersonalityChoice { NEUTRAL, HUMAN, EAGLE, LION, OX}

[System.Serializable]
public class DialogNodeJSON
{
    public string text;
    public List<JSONLinks> links;
    public string name;
    public int pid;
    public Vector2 position;
}

[System.Serializable]
public struct JSONLinks
{
    public string name;
    public string link;
    public bool broken;
}