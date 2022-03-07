using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Text.RegularExpressions;

[CreateAssetMenu(fileName = "New Dialog Graph Generator"
                , menuName = "Dialog/Graph Generator")]
[System.Serializable]
public class DialogGraphGenerator : ScriptableObject
{
    [SerializeField] CastList castList;
    [SerializeField, TextArea] string DialogStoryPath = "Assets/Scripts/ScriptableObjects/Story/";
    [SerializeField, TextArea] string TagActionFilePath = "Assets/Scripts/ScriptableObjects/TagActions/";
    const string COMMENT_SYMBOL = "//";
    const string LINK_SYMBOL = "[[";
    const string TAG_SYMBOL = "##";
    const char ACTION_SYMBOL = '@';
    // ##Eon_picture_ad
    // ##Stranger_anim_laugh

    [SerializeField] string folderName;
    [Button("Generate Dialog Graph")]
    void GenerateDialogGraph(TextAsset file)
    {
        if (file == null)
        {
            LogMessage(LogType.ERROR, "File is null");
            return;
        }
        DialogNodeList newGraph = JsonUtility.FromJson<DialogNodeList>(file.text);
        
        DialogGraph asset = ScriptableObject.CreateInstance<DialogGraph>();

        asset.name = newGraph.name;
        foreach (DialogNodeJSON node in newGraph.passages)
        {
            DialogNode dialogNode = ConvertJSONNodeToDialogNode(node);
            asset.nodes.Add(dialogNode);
        }
        AssetDatabase.CreateAsset(asset, DialogStoryPath + folderName + "/" + newGraph.name + ".asset");

        Debug.Log("Dialog Graphh " + newGraph + " was created.");
        Debug.Log("File Path: " + DialogStoryPath + folderName);
    }


    //maybe tags are like
    // tag_profile_Sad
    // tag_animation_Animationname

    DialogNode ConvertJSONNodeToDialogNode(DialogNodeJSON JSONnode)
    {

        DialogNode newNode = ScriptableObject.CreateInstance<DialogNode>();
        newNode.name = newNode.NodeTitle = JSONnode.name.Trim();

        // parse text
        // ignore lines that start with double slashes
        // each new line has the name of character that will be talking
        // the end of the line has tags
        List<string> dialogLines = new List<string>();
        //  Split at new line character
        List<string> nodeText = new List<string>();
        nodeText = new List<string>(JSONnode.text.Split('\n'));
        foreach (string line in nodeText)
        {
            //  Skip lines with less that 2 characters
            if (line.Length < 2) continue;
            //  If a line is a comment, place it in the notes section of the node
            else if(CommentCheck(line))
            {
                newNode.Notes += line;
            }
            else if (ActionCheck(line))
            {
                TagAction newAction = GenerateTagAction(line);
                newNode.AddTagAction(newAction);
            }
            //  If a line is a link, create the link info and add them to the newNode's
            //  nextNodes
            else if(LinkCheck(line))
            {
                LinkInfo linkInfo = CreateLinkInfo(line);
                newNode.nextNodes.Add(linkInfo.personality, linkInfo.node);
            }
            else
            {
                DialogLine dialogLine = GetDialogLine(line);
                newNode.speakingLines.Add(dialogLine);
            }
        }

        AssetDatabase.CreateAsset(newNode, DialogStoryPath + folderName + "/"  + "Nodes/" + newNode.name + ".asset");

        return newNode;

    }

    LinkInfo CreateLinkInfo(string rawLine)
    {
        string sanitizedLine = rawLine.Replace("[", "");
        sanitizedLine = sanitizedLine.Replace("]", "");
        string[] rawLinkInfo = sanitizedLine.Split('|');

        // Remove any white space at the beginning and end of links
        for(int i = 0; i < rawLinkInfo.Length; i++)
        {
            rawLinkInfo[i] = rawLinkInfo[i].Trim();
        }

        // Set up link info, by default the personality choice is neutral
        LinkInfo linkInfo;
        linkInfo.personality = PersonalityChoice.NEUTRAL;

        if (rawLinkInfo.Length > 1)
        {
            // Cycle through each personality choice
            foreach (PersonalityChoice pc in Enum.GetValues(typeof(PersonalityChoice)))
            {
                if (pc.ToString().ToUpper() == rawLinkInfo[0].ToUpper())
                {
                    linkInfo.personality = pc;
                    break;
                }
            }
        }
        //  The name of the link should always be the last string in the sanitized info
        int nodeNameIndex = rawLinkInfo.Length - 1;

        DialogNode existingNode = (DialogNode)AssetDatabase.LoadAssetAtPath(DialogStoryPath + folderName + "/" + "Nodes/" + rawLinkInfo[nodeNameIndex].Trim() + ".asset", typeof(DialogNode));
        if (existingNode)
        {
            linkInfo.node = existingNode;
        }
        else
        {
            //  Create instance of dialog node
            DialogNode node = ScriptableObject.CreateInstance<DialogNode>();
            //  Set the name of the file and not title to the name
            node.name = node.NodeTitle = rawLinkInfo[nodeNameIndex].Trim();

            //  Create aset and save it to its path
            AssetDatabase.CreateAsset(node, DialogStoryPath + folderName + "/" + "Nodes/" + node.name + ".asset");
            linkInfo.node = node;
        }
        return linkInfo;
    }

    public DialogLine GetDialogLine(string text)
    {
        string character = "Narrator";
        string c_text = text;

        List<string> lineData = new List<string>();
        int speakerSplitIndex = c_text.IndexOf(":");
        if (c_text.Contains(":"))
        {
            string[] rawTextInfo = c_text.Split(new[] { ':' }, 2);
            character = rawTextInfo[0];

            c_text = rawTextInfo[1];
            lineData.Add(character);
        }
        else
        {
            character = "Narrator";
        }


        DialogLine dialogLine = new DialogLine();
        dialogLine.speaker = castList.GetCharacter(character);
        string[] dialogAndTags = Regex.Split(c_text, @"(?=[@])");

        foreach (string line in dialogAndTags)
        {
            if (line[0] != '@')
            {
                dialogLine.line = line;
            }
            else
            {
                TagAction action = GenerateTagAction(line);
                dialogLine.AddTagAction(action);
            }
        }

        return dialogLine;
    }

    /*
     *      TODO: 
     *              -   Establish file oath to store all tag actions
     *              -   create tag action asset and link it to repective node/line
     *              
     * 
     * 
     */ 

    TagAction GenerateTagAction(string line)
    {
        string fileName = line.Replace("@", "");
        line = line.Replace("@", "");
        string[] rawLineInfo = line.Split('-');
        for(int i = 0; i < rawLineInfo.Length; i++)
        {
            rawLineInfo[i] = rawLineInfo[i].Trim();
            rawLineInfo[i] = rawLineInfo[i].Replace("-", "");
        }
        string rawType = rawLineInfo[0];

        Tags type = GetTagType(rawType);

        CharacterData character;

        TagAction tagAction = ScriptableObject.CreateInstance<TagAction>();
        tagAction.name = fileName;
        string typeOfAction = type.ToString();
        switch (type)
        {
            case Tags.ANIM:
                character = GetCharacter(rawLineInfo[1]);
                ((AnimationAction)tagAction).Init(character, rawLineInfo[2]);
                break;
            case Tags.SFX:
                ((PlaySFXAction)tagAction).Init(rawLineInfo[1]);
                break;
            case Tags.BGM:
                ((PlayBGMAction)tagAction).Init(rawLineInfo[1]);
                break;
            case Tags.PROFILE:
                character = GetCharacter(rawLineInfo[1]);
                ((ChangeProfilePictureAction)tagAction).Init(character, rawLineInfo[2]);
                break;
            case Tags.SET_VALUE:
                break;
            default:
                return null;
        }

        // TODO: create folders for tagactions
        AssetDatabase.CreateAsset(tagAction, TagActionFilePath + typeOfAction + "/" + tagAction.name + ".asset");


        // switch statement for different tag actions
        return null;
    }

    bool CommentCheck(string line)
    {
        string commentCheck = line.Substring(0, 2);
        return commentCheck == COMMENT_SYMBOL;
    }

    bool LinkCheck(string line)
    {
        string linkCheck = line.Substring(0, 2);
        return linkCheck == LINK_SYMBOL;
    }

    bool TagCheck(string line)
    {
        string tagCheck = line.Substring(0, 2);
        return tagCheck == TAG_SYMBOL;
    }

    bool ActionCheck(string line)
    {
        return line[0] == ACTION_SYMBOL;
    }

    Tags GetTagType(string value)
    {
        foreach (Tags tag in Enum.GetValues(typeof(Tags)))
        {
            if(value.ToUpper() == tag.ToString().ToUpper())
            {
                return tag;
            }
        }
        LogMessage(LogType.ERROR, "Tag " + value + " not found");
        return Tags.ERROR;
    }

    CharacterData GetCharacter(string name)
    {
        foreach (CharacterData character in Services.CastList.Characters)
        {
            if (name.ToUpper() == character.characterName.ToString().ToUpper())
            {
                return character;
            }
        }
        LogMessage(LogType.ERROR, "Character " + name + " not found in Cast List");
        return null;
    }

    struct LinkInfo
    {
        public PersonalityChoice personality;
        public DialogNode node;
    }

    public enum Tags { ERROR, ANIM, SFX, BGM, SET_VALUE, PROFILE}

    public enum LogType { MESSAGE, WARNING, ERROR }
    public void LogMessage(LogType messageType, string message)
    {
        Debug.Log("[" + messageType + "]: " + message);
    }
}
