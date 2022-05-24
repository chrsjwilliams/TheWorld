using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;

// ~TODO:   LION ABILITY
//          HUMAN ABILITY

public class DialogGraphParser : MonoBehaviour
{
    [Header("Display all lines is not working yet, leave false for now")]
    public bool displayAllLines;
    private static StoryCharacter eon;

    public bool finishedAnimating;

    [Space(25)]
    [SerializeField] CastList castList;
    [SerializeField] List<StoryCharacter> characters;
    [SerializeField] CharacterData player;
    [SerializeField] CharacterData narrator;
    [SerializeField] DialogGraph dialogGraph;

    [Space(25)]
    [SerializeField] DialogNode currentNode;

    [Space(25)]
    [SerializeField] List<DialogNode> visitedNodes = new List<DialogNode>();
    public List<DialogNode> VisitedNodes { get { return visitedNodes; } }

    [Space(25)]
    [SerializeField] TextMeshProUGUI nodeNameText;
    [SerializeField] TextMeshProUGUI dialogText;
    [SerializeField] TextMeshProUGUI backArrowCounter;
    [SerializeField] HorizontalLayoutGroup horizontalLayoutGroup;
    [SerializeField] ToggleButtonUsingBoolSO backButton;
    int backArrowCount;

    [Space(25)]
    [SerializeField] Image playerProfile;
    [SerializeField] Image npcProfile;
    [SerializeField] Image wheelProfile;
    [SerializeField] Image nodeImage;

    [Space(25)]
    [SerializeField] List<TextBox> textBoxes;
    TextBox selectedTexBox;

    [Space(25)]
    [SerializeField] CanvasGroup playerProfileCanvasGroup;
    [SerializeField] CanvasGroup npcProfileCanvasGroup;
    [SerializeField] CanvasGroup characterSelection;

    [Space(25)]
    [SerializeField] SpriteRenderer background;

    [Space(25)]
    [SerializeField] DialogButton selectedButton;
    [SerializeField] List<DialogButton> unselectedButtons;
    List<DialogButton> allButtons;

    [Space(25)]
    public int dialogLineIndex = 0;
    [SerializeField] BoolVariable CanMakeSelection;
    [SerializeField] BoolVariable MadeSelection;

    [Space(25)]
    [SerializeField] MonoTweener fadeInScreenCover;

    [Space(25)]
    [SerializeField] SimpleEvent AllowPlayerCharacterInteraction;
    [SerializeField] SimpleEvent BlockPlayerCharacterInteraction;

    const string COLOUR_TAG = " (text-colour: ";
    const string COLOR_TAG = " (text-color: ";

    ShuffleBag<int> intBag = new ShuffleBag<int>();

    public void FinishedAnimating(bool b)
    {
        finishedAnimating = b;
    }

    public void NextButtonPressed()
    {
        if (AtLastLineOfDialog() || displayAllLines)
        {
            if(currentNode.nextNodes.Count < 1)
            {
                fadeInScreenCover?.Play();
                return;
            }
            MoveToNextNode();
            DisplayDialogChoices();
        }
        else
        {
            DisplayNextDialogLine();
        }
    }

    public void PreviousButtonPressed()
    {
        if (backArrowCount == 0 || visitedNodes.Count < 2)
        {
            Services.AudioManager.PlayClip(SFX.ERROR);
            return;
        }
        backArrowCount -= 1;
        backArrowCounter.text = backArrowCount + "";

        visitedNodes.RemoveAt(0);
        // Go backa node
        Debug.Log("~~~LEAVING TO NODE: " + currentNode.name);
        var prevNode = visitedNodes[0];
        Debug.Log("~~~GOING TO NODE: " + currentNode.name);
        GoToVisitedNode(prevNode);
        if (backArrowCount < 1)
        {
            backButton.turnOffButton = true;
        }
    }

    public void StartStory(TransitionData data)
    {
        
        for (int i = 0; i < textBoxes.Count; i++)
        {
            intBag.Add(i);
        }

        dialogGraph = data.selectedStory;
        castList = data.selectedCastList;
        Services.SetCurrentCast(castList);

        background.sprite = data.selectedStory.storyBackground;
        if (currentNode != null)
        {
            Debug.LogError("Current Node is not empty");
            return;
        }

        if(!dialogGraph.MustMakeSelection)
        {
            MadeSelection.value = true;
        }

        if (eon == null)
        {
            var player = Services.CastList.Player;
            eon = Instantiate(player.Model);
            Services.CastList.AddCharacterModel(eon);
        }

        currentNode = dialogGraph.startingNode;
        nodeNameText.text = currentNode.NodeTitle;
        visitedNodes.Clear();
        visitedNodes.Insert(0, currentNode);
        currentNode.ExecuteTagActions(() => { });
        dialogLineIndex = 0;
        if (!displayAllLines)
        {
            DisplayNextDialogLine();
        }
        else
        {
            DisplayAllNodeText();
        }
        DisplayDialogChoices();
        Services.AudioManager.PlayBGM(dialogGraph.music);
        backArrowCount = 3;
        backArrowCounter.text = backArrowCount + "";
        backButton.turnOffButton = false;
        //HideAllDialogButtons();
    }

    void MoveToNextNode()
    {
        if (!finishedAnimating)
        {
            Services.AudioManager.PlayClip(SFX.ERROR);
            return;
        }

        if (CanMakeSelection.value && !MadeSelection.value && !displayAllLines) return;
        if (currentNode.HasPersonalityChoice(selectedButton.PersonalityChoice))
        {
            Services.AudioManager.PlayClip(SFX.CLICK);

            npcProfileCanvasGroup.alpha = 0;

            //  Set current node to next node
            if (currentNode.IsNeutralNode())
            {
                currentNode = currentNode.nextNodes[PersonalityChoice.NEUTRAL];
            }
            else
            {
                currentNode = currentNode.nextNodes[selectedButton.PersonalityChoice];
            }

            if(currentNode == null)
            {
                Debug.LogError("Current node is null for some reason!");
            }
            nodeImage.sprite = currentNode.nodeSprite;

            nodeNameText.text = currentNode.NodeTitle;
            //  add new node to node stack
            visitedNodes.Insert(0, currentNode);
            // execute actions on the node
            currentNode.ExecuteTagActions(() => { });
            //  reset dialog line index
            dialogLineIndex = 0;
            //  display dialog line
            DisplayNextDialogLine();
            if (dialogGraph.MustMakeSelection)
            {
                MadeSelection.value = false;
            }
            CanMakeSelection.value = true;
            if (!currentNode.IsNeutralNode() && currentNode.nextNodes.Count > 1)
            {
                AllowPlayerCharacterInteraction?.Raise();
            }
        }
        else
        {
            Debug.LogError("Choice " + selectedButton.PersonalityChoice.ToString() +
                " not avialable for " + currentNode.name + " node");
        }
    }

    void DisplayNextDialogLine()
    {
        DialogLine currentLine = currentNode.speakingLines[dialogLineIndex];

        CharacterData character = currentLine.speaker;   

        //dialogText.text = currentNode.speakingLines[dialogLineIndex].line;
        SetNodeText();



        RefreshLayoutGroup();
        currentLine.ExecuteTagActions(() =>{ });
        dialogLineIndex++;
    }

    void DisplayDialogChoices()
    {

        if (currentNode.nextNodes.Count == 1)
        {
            MadeSelection.value = true;
            CanMakeSelection.value = false;
            BlockPlayerCharacterInteraction?.Raise();

            characterSelection.alpha = 0;
            characterSelection.interactable = false;
        }
        else if (currentNode.nextNodes.Count > 1)
        {
            characterSelection.alpha = 1;
            characterSelection.interactable = true;
        }

        if (currentNode.IsNeutralNode() && currentNode != dialogGraph.startingNode) return;

        //  Check if current node has our current personality choice selected
        //  If it doesn't, then we need to swap to the first available choice
        if (!currentNode.nextNodes.ContainsKey(selectedButton.PersonalityChoice))
        {
            // Current Node has 1 dalog choice that IS NOT our previously
            // selected personality choice
            // There should be only 1 key, therefore we should only go through
            // this loop once
            foreach (KeyValuePair<PersonalityChoice, DialogNode> entry in currentNode.nextNodes)
            {
                SwapSelectedButtonIcon(entry.Key);
                break;
            }
        }

        // hide all unused choices
        HideUnusedDialogButtons(currentNode.nextNodes);
    }

    private void HideUnusedDialogButtons(Dictionary<PersonalityChoice, DialogNode> choices)
    {
        foreach(DialogButton button in unselectedButtons)
        {
            if(!choices.ContainsKey(button.PersonalityChoice))
            {
                button.ShowDialogButton(false);
            }
            else
            {
                button.ShowDialogButton(true);

            }
        }
    }

    private void HideAllDialogButtons()
    {
        foreach (DialogButton button in unselectedButtons)
        {

            button.ShowDialogButton(false);
        }
    }

    bool AtLastLineOfDialog()
    {
        return true;
        //return dialogLineIndex == currentNode.speakingLines.Count - 1;
    }

    void ChangeSelectedButtonIcon(PersonalityChoice newChoice)
    {
        MadeSelection.value = true;
        //CanMakeSelection.value = false;
        //BlockPlayerCharacterInteraction?.Raise();
        SwapSelectedButtonIcon(newChoice);

    }

    void SwapSelectedButtonIcon(PersonalityChoice newChoice)
    {
        // Get the previoius personality choice
        PersonalityChoice oldChoice = selectedButton.PersonalityChoice;

        // If the choices are the same or if the choice is neutral, exit
        if (newChoice == oldChoice || newChoice == PersonalityChoice.NEUTRAL)
        {
            return;
        }

        // Find the button that has our current choice
        // we are going to swap its sprite wiht our old choice
        DialogButton oldButton = null;
        foreach(DialogButton button in unselectedButtons)
        {
            if(button.PersonalityChoice == newChoice)
            {
                oldButton = button;
            }
        }

        // Swap the image sprites
        oldButton.SetPersonalityChoice(oldChoice);
        selectedButton.SetPersonalityChoice(newChoice);

        selectedTexBox.playerImage.sprite = wheelProfile.sprite;

    }
    bool hasNPCDialog = false;
    bool onlyNarratorDialog = true;
    void DisplayAllNodeText()
    {
        string nodetext = "";
        foreach(DialogLine line in currentNode.speakingLines)
        {
            nodetext += line.line + "\n";
        }  
        dialogText.text = nodetext;
    }

    TextBox SelectTextBox(string message)
    {
        int index = intBag.Next();
        TextBox textBox = null;
        while(textBox == null)
        {
            if(textBoxes[index].characterLimit > message.Length)
            {
                textBox =  textBoxes[index];
                break;
            }

            index = intBag.Next();
        }

        return textBox;
    }

    void SetNodeText()
    {
        string nodetext = "";
        foreach (DialogLine line in visitedNodes[0].speakingLines)
        {
            if(line.speaker != narrator && line.speaker != player)
            {
                hasNPCDialog = true;
                npcProfile.sprite = line.speaker.GetCharacterProfile(Emote.NEUTRAL);

            }
            else if(line.speaker != narrator)
            {
                onlyNarratorDialog = false;
            }

            if (line.line.Contains(COLOR_TAG) || line.line.Contains(COLOUR_TAG))
            {
                npcProfileCanvasGroup.alpha = 1;

                if (line.line.Contains(COLOR_TAG))
                {
                    line.line = line.line.Replace(COLOR_TAG, "<color=");
                }
                else
                {
                    line.line = line.line.Replace(COLOUR_TAG, "<color=");
                }
                line.line = line.line.Replace(")", ">");

                line.line = line.line.Replace("[", "");
                line.line = line.line.Replace("]", "");

                line.line += "</color>";

            }

            if(line.line.Contains("<color=") && line.line.Contains("</color>"))
            {
                line.line += "</color>";
            }
            nodetext += line.line + "\n";
            line.ExecuteTagActions(() => { });
        }

        selectedTexBox = SelectTextBox(nodetext);
        selectedTexBox.playerImage.sprite = wheelProfile.sprite;

        foreach (TextBox textBox in textBoxes)
        {
            if(textBox != selectedTexBox)
            {
                textBox.ShowTextBox(false);
            }
        }


        if (onlyNarratorDialog)
        {
            selectedTexBox.DisplayPicture(TextBox.PictureTpe.NONE);
        }
        else if (hasNPCDialog)
        {
            selectedTexBox.DisplayPicture(TextBox.PictureTpe.NPC);
        }
        else
        {
            selectedTexBox.DisplayPicture(TextBox.PictureTpe.PLAYER);
        }
        selectedTexBox.SetText(nodetext);
    }


    public void GoToVisitedNode(DialogNode node)
    {
        if(visitedNodes.Contains(node))
        {
            currentNode = node;

            nodeNameText.text = currentNode.NodeTitle;
            //  add new node to node stack

            // execute actions on the node
            currentNode.ExecuteTagActions(() => { });
            //  reset dialog line index
            dialogLineIndex = 0;
            //  display dialog line
            DisplayNextDialogLine();
            if (dialogGraph.MustMakeSelection)
            {
                MadeSelection.value = false;
            }
            CanMakeSelection.value = true;
            if (!currentNode.IsNeutralNode() && currentNode.nextNodes.Count > 1)
            {
                AllowPlayerCharacterInteraction?.Raise();
            }
            DisplayDialogChoices();
        }
        else
        {
            Debug.LogError("Dialog Node " + node.name + " has not been visited");
        }
    }

    public List<DialogNode> GetNodesForAbility(PersonalityChoice choice)
    {
        List<DialogNode> nodes = new List<DialogNode>();

        switch(choice)
        {
            case PersonalityChoice.HUMAN:

                List<DialogNode> humanNodes = GetNextNodes(visitedNodes[0]);

                List<DialogNode> nodesToRemove = new List<DialogNode>();
                foreach(DialogNode node in humanNodes)
                {
                    if(node != currentNode && node.nodeType != PersonalityChoice.HUMAN)
                    {
                        nodesToRemove.Add(node);
                    }
                }

                foreach(DialogNode node in nodesToRemove)
                {
                    humanNodes.Remove(node);
                }

                nodes.AddRange(humanNodes);
                break;
            case PersonalityChoice.EAGLE:
                break;
            case PersonalityChoice.LION:
                nodes.AddRange(visitedNodes);
                break;
            case PersonalityChoice.OX:
                break;
            default:
                break;
        }


        return nodes;
    }

    public DialogNode GetNode(string nodeName)
    {
        foreach(DialogNode node in dialogGraph.nodes)
        {
            if(nodeName == node.name)
            {
                return node;
            }
        }

        return null;
    }

    public DialogNode FindSpecialItem()
    {
        foreach(DialogNode node in dialogGraph.nodes)
        {
            if (node.hasSecretItem)
                return node;
        }

        return null;
    }


    public List<DialogNode> GetNextNodes(DialogNode startingNode)
    {
        // Mark all the vertices as not visited
        // (set as false by default in c#)
        List<DialogNode> visited = new List<DialogNode>();


        // Call the recursive helper function
        // to print DFS traversal
        List<DialogNode> nextNodes = DFSUtil(startingNode, visited);

        return nextNodes;
    }


    List<DialogNode> DFSUtil(DialogNode v, List<DialogNode> visited)
    {


        // Mark the current node as visited
        // and print it
        visited.Add(v);

        // Recur for all the vertices
        // adjacent to this vertex
        List<DialogNode> vList = v.GetNextNodes();
        if (vList != null)
        {
            foreach (var n in vList)
            {
                if (!visited.Contains(n))
                    DFSUtil(n, visited);
            }
        }

        return visited;
    }

    void RefreshLayoutGroup()
    {
        Canvas.ForceUpdateCanvases();
        horizontalLayoutGroup.enabled = false;
        horizontalLayoutGroup.enabled = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        DialogButton.DialogButtonPressed += ChangeSelectedButtonIcon;
        allButtons = new List<DialogButton>();
        allButtons.Add(selectedButton);
        allButtons.AddRange(unselectedButtons);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            NextButtonPressed();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            PreviousButtonPressed();
        }

    }
}
