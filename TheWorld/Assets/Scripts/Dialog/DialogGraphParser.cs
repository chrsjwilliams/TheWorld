using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;

public class DialogGraphParser : MonoBehaviour
{
    [Header("Display all lines is not working yet, leave false for now")]
    public bool displayAllLines;

    [Space(25)]
    [SerializeField] CastList castList;
    [SerializeField] List<StoryCharacter> characters;
    [SerializeField] CharacterData player;
    [SerializeField] CharacterData narrator;
    [SerializeField] DialogGraph dialogGraph;

    [SerializeField] DialogNode currentNode;

    [SerializeField] Stack<DialogNode> visitedNodes = new Stack<DialogNode>();

    [SerializeField] TextMeshProUGUI nodeNameText;
    [SerializeField] TextMeshProUGUI dialogText;
    [SerializeField] HorizontalLayoutGroup horizontalLayoutGroup;

    [SerializeField] Image playerProfile;
    [SerializeField] Image npcProfile;
    [SerializeField] Image wheelProfile;

    [SerializeField] CanvasGroup playerProfileCanvasGroup;
    [SerializeField] CanvasGroup npcProfileCanvasGroup;


    [SerializeField] DialogButton selectedButton;
    [SerializeField] List<DialogButton> unselectedButtons;
    List<DialogButton> allButtons;

    public int dialogLineIndex = 0;
    [SerializeField] BoolVariable CanMakeSelection;
    [SerializeField] BoolVariable MadeSelection;


    [SerializeField] SimpleEvent AllowPlayerCharacterInteraction;
    [SerializeField] SimpleEvent BlockPlayerCharacterInteraction;

    const string COLOUR_TAG = " (text-colour: ";
    const string COLOR_TAG = " (text-color: ";

    // Choices we have not selected
    // last choice selected

    public void NextButtonPressed()
    {
        if (AtLastLineOfDialog() || displayAllLines)
        {
            
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

    }

    public void StartStory(TransitionData data)
    {
        dialogGraph = data.selectedStory;
        castList = data.selectedCastList;
        Services.SetCurrentCast(castList);
        
        if (currentNode != null)
        {
            Debug.LogError("Current Node is not empty");
            return;
        }

        dialogGraph.Init();
        var player = Services.CastList.Player;
        var character = Instantiate(player.Model);
        Services.CastList.AddCharacterModel(character);

        currentNode = dialogGraph.startingNode;
        nodeNameText.text = currentNode.NodeTitle;
        visitedNodes.Clear();
        visitedNodes.Push(currentNode);
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
        playerProfile.sprite = wheelProfile.sprite;

        //HideAllDialogButtons();
    }

    [Button]
    public void StartStory()
    {
        if (currentNode != null)
        {
            Debug.LogError("Current Node is not empty");
            return;
        }

        dialogGraph.Init();
        currentNode = dialogGraph.startingNode;
        nodeNameText.text = currentNode.NodeTitle;
        visitedNodes.Clear();
        visitedNodes.Push(currentNode);
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
        HideAllDialogButtons();

    }

    void MoveToNextNode()
    {
        if (CanMakeSelection.value && !MadeSelection.value && !displayAllLines) return;
        if (currentNode.HasPersonalityChoice(selectedButton.PersonalityChoice))
        {
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
            nodeNameText.text = currentNode.NodeTitle;
            //  add new node to node stack
            visitedNodes.Push(currentNode);
            // execute actions on the node
            currentNode.ExecuteTagActions(() => { });
            //  reset dialog line index
            dialogLineIndex = 0;
            //  display dialog line
            DisplayNextDialogLine();

            MadeSelection.value = false;
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

        if(character.IsPlayer)
        {
            playerProfileCanvasGroup.alpha = 1;
            npcProfileCanvasGroup.alpha = 0;
            //playerProfile.sprite = character.GetCharacterProfile(Emote.NEUTRAL);
        }
        else if(character.IsNarrator)
        {
            npcProfileCanvasGroup.alpha = 0;
            playerProfileCanvasGroup.alpha = 0;
        }
        else
        {
            playerProfileCanvasGroup.alpha = 0;
            npcProfileCanvasGroup.alpha = 1;
            //npcProfile.sprite = character.GetCharacterProfile(Emote.NEUTRAL);
        }

        //dialogText.text = currentNode.speakingLines[dialogLineIndex].line;
        dialogText.text = GetAllNodeText();
        RefreshLayoutGroup();
        currentLine.ExecuteTagActions(() =>{ });
        dialogLineIndex++;
    }

    void DisplayDialogChoices()
    {
        playerProfile.sprite = wheelProfile.sprite;

        if (currentNode.nextNodes.Count == 1)
        {
            MadeSelection.value = true;
            CanMakeSelection.value = false;
            BlockPlayerCharacterInteraction?.Raise();
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
        // Go through all the unselected buttons and hide them if we cannot use them
        // ~TODO: have each button tween away/in before changing their active status
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

        playerProfile.sprite = wheelProfile.sprite;

    }

    void DisplayAllNodeText()
    {
        string nodetext = "";
        foreach(DialogLine line in currentNode.speakingLines)
        {
            nodetext += line.line + "\n";
        }
        dialogText.text = nodetext;

    }

    string GetAllNodeText()
    {
        string nodetext = "";
        foreach (DialogLine line in currentNode.speakingLines)
        {
            if(line.speaker != narrator && line.speaker != player)
            {
                npcProfileCanvasGroup.alpha = 1;
            }

            if (line.line.Contains(COLOR_TAG) || line.line.Contains(COLOUR_TAG))
            {
                npcProfileCanvasGroup.alpha = 1;
                //string[] rawTextInfo = c_text.Split(new[] { ':' }, 2);
                //string[] colorInfo = c_text.Split(new[] { ')' }, 2);
                //(text-color:

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
        return nodetext;
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
