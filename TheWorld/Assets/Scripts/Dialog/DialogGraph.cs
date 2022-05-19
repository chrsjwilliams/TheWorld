using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "New Dialog Graph"
                , menuName = "Dialog/Graph")]
[System.Serializable]
public class DialogGraph : SerializedScriptableObject
{
    public bool MustMakeSelection;
    public BGM music;

    public DialogNode startingNode;

    public List<DialogNode> nodes = new List<DialogNode>();

    public Sprite storyBackground;

    [Button]
    public void SetNodeTypes()
    {
        foreach(DialogNode node in nodes)
        {
            node.SetNodeTypes();
        }
        Debug.Log("[LOG MESSAGE] Node types set.");
    }
}

[System.Serializable]
public class DialogNodeList
{
    [SerializeField] public string name;
    [SerializeField] public List<DialogNodeJSON> passages;
}