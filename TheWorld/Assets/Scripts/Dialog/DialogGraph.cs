using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "New Dialog Graph"
                , menuName = "Dialog/Graph")]
[System.Serializable]
public class DialogGraph : SerializedScriptableObject
{
    public DialogNode startingNode;

    public DialogNode previoslyVisitedNode;

    public List<DialogNode> nodes = new List<DialogNode>();

    public DialogNodeList passages;
}

[System.Serializable]
public class DialogNodeList
{
    [SerializeField] public string name;
    [SerializeField] public List<DialogNodeJSON> passages;
}