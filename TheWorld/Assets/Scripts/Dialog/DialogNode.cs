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
    [SerializeField] public bool hasSecretItem;
    [SerializeField] public PersonalityChoice nodeType;
    [SerializeField] public Sprite nodeSprite;
    [SerializeField] public string NodeTitle;
    public List<DialogLine> speakingLines = new List<DialogLine>();
    public Dictionary<PersonalityChoice, DialogNode> nextNodes = new Dictionary<PersonalityChoice, DialogNode>();

    [TextArea]
    [SerializeField] public string Notes;

    [SerializeField] List<TagAction> lineActions = new List<TagAction>();


    public void SetNodeTypes()
    {
        foreach (KeyValuePair<PersonalityChoice, DialogNode> entry in nextNodes)
        {
            entry.Value.nodeType = entry.Key;
        }
    }

    public void AddTagAction(TagAction action)
    {
        lineActions.Add(action);
    }

    public void ExecuteTagActions(Action callback)
    {
        foreach (TagAction lineAction in lineActions)
        {
            // Temporarily only doing animation actions
            if(lineAction is AnimationAction)
                lineAction.ExecuteAction();
        }

        callback?.Invoke();
    }

    public List<DialogNode> GetNextNodes()
    {
        List<DialogNode> nextNodesList = new List<DialogNode>();
        foreach(DialogNode node in nextNodes.Values)
        {
            nextNodesList.Add(node);
        }

        return nextNodesList;
    }

    public bool HasPersonalityChoice(PersonalityChoice choice)
    {
        return nextNodes.ContainsKey(choice) || IsNeutralNode();
    }

    public bool IsNeutralNode()
    {
        return nextNodes.ContainsKey(PersonalityChoice.NEUTRAL);
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