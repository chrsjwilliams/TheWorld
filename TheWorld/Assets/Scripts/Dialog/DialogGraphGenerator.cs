using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "New Dialog Graph Generator"
                , menuName = "Dialog/Graph Generator")]
[System.Serializable]
public class DialogGraphGenerator : ScriptableObject
{
    [Button("Generate Dialog Graph")]
    void GenerateDialogGraph(TextAsset file)
    {
        if (file == null)
        {
            LogMessage(LogType.ERROR, "File is null");
            return;
        }
        DialogNodeList newGraph = JsonUtility.FromJson<DialogNodeList>(file.text);

        foreach (DialogNodeJSON node in newGraph.passages)
        {
            Debug.Log(node.links);
            //foreach(string link in node.links)
            //{
            //    Debug.Log(link);
            //}
        }

    }

    //maybe tags are like
    // tag_profile_Sad
    // tag_animation_Animationname

    void ConvertJSONNodeToDialogNode(DialogNodeJSON node)
    {
        // check if node with name exists,
        // if it does overwrite info
        // otherwise continue

        DialogNode newNode = ScriptableObject.CreateInstance<DialogNode>();

        newNode.name = newNode.NodeTitle = node.name;
        // parse text
        // ignore lines that start with double slashes
        // each new line has the name of character that will be talking
        // the end of the line has tags

        //create new links
        // check if dialognode with link name exists
        // if it does, do nothing
        // otherwise create new empty node

    }

    public enum LogType { MESSAGE, WARNING, ERROR }
    public void LogMessage(LogType messageType, string message)
    {
        Debug.Log("[" + messageType + "]: " + message);
    }
}
