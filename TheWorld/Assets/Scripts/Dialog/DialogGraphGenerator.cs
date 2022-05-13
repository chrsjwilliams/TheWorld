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
    const string NODE_PICURE_SYMBOL = "**";
    const string SECRET_ITEM_KEY = "~~";

    [SerializeField] SpriteLookUpEnum nodePictureDictionary;

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
        if(PictureSymbolCheck(nodeText[0]))
        {
            SetNodeSprite(nodeText[0], newNode);
            nodeText.RemoveAt(0);
        }
        else
        {
            SetNodeSprite("DEFAULT", newNode);
        }

        if(nodeText.Count > 2 && (SecretItemSymbolCheck(nodeText[1]) || SecretItemSymbolCheck(nodeText[0])))
        {
            newNode.hasSecretItem = true;
        }

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

    public DialogNode GetNode(string name)
    {
        DialogNode existingNode = (DialogNode)AssetDatabase.LoadAssetAtPath(DialogStoryPath + folderName + "/" + "Nodes/" + name + ".asset", typeof(DialogNode));
        return existingNode;
    }

    LinkInfo CreateLinkInfo(string rawLine)
    {
        string sanitizedLine = rawLine.Replace("[", "");
        sanitizedLine = sanitizedLine.Replace("]", "");
        string[] rawLinkInfo = sanitizedLine.Split('|');

        // Remove any white space at the beginning and end of links
        for (int i = 0; i < rawLinkInfo.Length; i++)
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

        DialogNode existingNode = GetNode(rawLinkInfo[nodeNameIndex].Trim());
        if (existingNode != null)
        {
            linkInfo.node = existingNode;
        }
        else
        {
            linkInfo.node = null;
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

    TagAction GenerateTagAction(string line)
    {
        string fileName = line.Replace("@", "");
        line = line.Replace("@", "");
        string[] rawLineInfo = line.Split(new[] { '_'}, 2);
        for(int i = 0; i < rawLineInfo.Length; i++)
        {
            rawLineInfo[i] = rawLineInfo[i].Trim();
            rawLineInfo[i] = rawLineInfo[i].Replace("-", "");
        }

        string rawType = rawLineInfo[0];

        Tags type = GetTagType(rawType);
        string typeOfAction = type.ToString();

        // If we already have an scriptable object for this tag action, use that
        TagAction exisitingAction = (TagAction)AssetDatabase.LoadAssetAtPath(TagActionFilePath + typeOfAction + "/" + fileName + ".asset", typeof(TagAction));
        if (exisitingAction)
            return exisitingAction;

        // IF acion doesn't already exist, create a new one
        CharacterData character;
        TagAction tagAction;
        
        switch (type)
        {
            case Tags.ANIM:
                tagAction = ScriptableObject.CreateInstance<AnimationAction>();
                character = GetCharacter(rawLineInfo[0]);
                ((AnimationAction)tagAction).Init(character, rawLineInfo[1]);
                tagAction.name = fileName;
                AssetDatabase.CreateAsset(tagAction, TagActionFilePath + typeOfAction + "/" + tagAction.name + ".asset");

                return tagAction;
            case Tags.SFX:
                tagAction = ScriptableObject.CreateInstance<PlaySFXAction>();
                ((PlaySFXAction)tagAction).Init(rawLineInfo[1]);
                tagAction.name = fileName;
                AssetDatabase.CreateAsset(tagAction, TagActionFilePath + typeOfAction + "/" + tagAction.name + ".asset");

                return tagAction;
            case Tags.BGM:
                tagAction = ScriptableObject.CreateInstance<PlayBGMAction>();
                ((PlayBGMAction)tagAction).Init(rawLineInfo[1]);
                tagAction.name = fileName;
                AssetDatabase.CreateAsset(tagAction, TagActionFilePath + typeOfAction + "/" + tagAction.name + ".asset");

                return tagAction;
            case Tags.PROFILE:
                tagAction = ScriptableObject.CreateInstance<ChangeProfilePictureAction>();
                character = GetCharacter(rawLineInfo[1]);
                ((ChangeProfilePictureAction)tagAction).Init(character, rawLineInfo[2]);
                tagAction.name = fileName;
                AssetDatabase.CreateAsset(tagAction, TagActionFilePath + typeOfAction + "/" + tagAction.name + ".asset");

                return tagAction;
            case Tags.END:
                return null;
            default:
                return null;
        }

    }

    void SetNodeSprite(string line, DialogNode node)
    {
        line = line.Replace(NODE_PICURE_SYMBOL, "").ToUpper();

        var key = GetNodePicEnum(line);
        Sprite value;
        if(nodePictureDictionary.TryGetSprite(key, out value))
        {
            node.nodeSprite = value;
        }
        else
        {
            Debug.Log("Sprite not found for value " + line);
        }
    }

    bool CommentCheck(string line)
    {
        string commentCheck = line.Substring(0, 2);
        return commentCheck == COMMENT_SYMBOL;
    }

    bool PictureSymbolCheck(string line)
    {
        string pictureSymbolCheck = line.Substring(0, 2);
        return pictureSymbolCheck == NODE_PICURE_SYMBOL;
    }

    bool SecretItemSymbolCheck(string line)
    {
        string secretItemSymbolCheck = line.Substring(0, 2);
        return secretItemSymbolCheck == SECRET_ITEM_KEY;
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
            if(value.ToUpper() == "EON")
            {
                return Tags.ANIM;
            }
        }
        LogMessage(LogType.ERROR, "Tag " + value + " not found");
        return Tags.ERROR;
    }

    CharacterData GetCharacter(string name)
    {
        foreach (CharacterData character in castList.Characters)
        {
            if (name.ToUpper() == character.characterName.ToString().ToUpper())
            {
                return character;
            }
        }
        LogMessage(LogType.ERROR, "Character " + name + " not found in Cast List");
        return null;
    }

    NODE_PICTURE GetNodePicEnum(string name)
    {
        foreach (NODE_PICTURE effect in Enum.GetValues(typeof(NODE_PICTURE)))
        {
            if (name.ToUpper() == effect.ToString().ToUpper())
            {
                return effect;
            }
        }
        LogMessage(LogType.ERROR, "Node Picture " + name + " not found in Node Picture enum list");
        return NODE_PICTURE.DEFAULT;
    }

    struct LinkInfo
    {
        public PersonalityChoice personality;
        public DialogNode node;
    }

    public enum Tags { ERROR, ANIM, SFX, BGM, SET_VALUE, PROFILE, END}


    public enum LogType { MESSAGE, WARNING, ERROR }
    public void LogMessage(LogType messageType, string message)
    {
        Debug.Log("[" + messageType + "]: " + message);
    }
}

public enum NODE_PICTURE {  DEFAULT, CLOCK, DIALOG, END, EXIT, FOOTSTEPS, GUESSLOVE, GUESSSTD,
                            GUESSWORD, ICONALERT, QUESTIONMARK, STRANGER, TURNIP, EAR, FIRE, EXXMARK, 
                            LUNCHBAG, CRY, EXPLOSION, LAPTOP, COFFEE, EYES, EXXQUESTION, SERPENT, SUN, 
                            BLUEBANANA, APPLE, BLUEAPPLE, HEART}
