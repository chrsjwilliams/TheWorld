using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;

public class DialogGraphParser : MonoBehaviour
{
    [SerializeField] CharacterData player;
    [SerializeField] CharacterData narrator;
    [SerializeField] DialogGraph dialogGraph;

    [SerializeField] DialogNode currentNode;

    [SerializeField] Stack<DialogNode> visitedNodes = new Stack<DialogNode>();

    [SerializeField] TextMeshProUGUI dialogText;

    [SerializeField] Image playerProfile;
    [SerializeField] Image npcProfile;

    [SerializeField] CanvasGroup playerProfileCanvasGroup;
    [SerializeField] CanvasGroup npcProfileCanvasGroup;


    [SerializeField] DialogButton selectedButton;
    [SerializeField] List<DialogButton> unselectedButtons;
    List<DialogButton> allButtons;

    public int dialogLineIndex = 0;
    public bool madeNodeSelection = false;


    // Choices we have not selected
    // last choice selected

    public void NextButtonPressed()
    {
        if (dialogLineIndex == currentNode.speakingLines.Count)
        {
            DisplayDialogChoices();
            MoveToNextNode();
        }
        else
        {
            DisplayNextDialogLine();
        }
    }

    public void PreviousButtonPressed()
    {

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
        visitedNodes.Clear();
        visitedNodes.Push(currentNode);
        currentNode.ExecuteTagActions(() => { });
        dialogLineIndex = 0;
        DisplayNextDialogLine();

    }

    void MoveToNextNode()
    {
        if (!madeNodeSelection) return;
        
        if (currentNode.HasPersonalityChoice(selectedButton.PersonalityChoice))
        {
            //  Set current node to next node
            if (currentNode.IsNeutralNode())
            {
                currentNode = currentNode.nextNodes[PersonalityChoice.NEUTRAL];
            }
            else
            {
                Debug.Log("Making Choice: " + selectedButton.PersonalityChoice);
                currentNode = currentNode.nextNodes[selectedButton.PersonalityChoice];
            }

            //  add new node to node stack
            visitedNodes.Push(currentNode);
            // execute actions on the node
            currentNode.ExecuteTagActions(() => { });
            //  reset dialog line index
            dialogLineIndex = 0;
            //  display dialog line
            DisplayNextDialogLine();

            madeNodeSelection = false;
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

        //if (character == Services.CastList.Player)
        if(character == player)
        {
            playerProfileCanvasGroup.alpha = 1;
            npcProfileCanvasGroup.alpha = 0;
            playerProfile.sprite = character.GetCharacterProfile(Emote.NEUTRAL);
        }
        else if(character == narrator)
        {
            npcProfileCanvasGroup.alpha = 0;
            playerProfileCanvasGroup.alpha = 0;
        }
        else
        {
            playerProfileCanvasGroup.alpha = 0;
            npcProfileCanvasGroup.alpha = 1;
            npcProfile.sprite = character.GetCharacterProfile(Emote.NEUTRAL);
        }

        dialogText.text = currentNode.speakingLines[dialogLineIndex].line;
        currentLine.ExecuteTagActions(() =>{ });
        dialogLineIndex++;
    }

    void DisplayDialogChoices()
    {
        if(currentNode.nextNodes.Count == 1)
        {
            madeNodeSelection = true;
        }

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
        // TODO: have each button tween away/in before changing their active status
        foreach(DialogButton button in unselectedButtons)
        {
            if(!choices.ContainsKey(button.PersonalityChoice))
            {
                button.gameObject.SetActive(false);
            }
            else
            {
                button.gameObject.SetActive(true);

            }
        }
    }

    bool AtLastLineOfDialog()
    {
        return dialogLineIndex == currentNode.speakingLines.Count - 1;
    }

    void ChangeSelectedButtonIcon(PersonalityChoice newChoice)
    {
        madeNodeSelection = true;
        SwapSelectedButtonIcon(newChoice);
    }

    void SwapSelectedButtonIcon(PersonalityChoice newChoice)
    {
        // Get the previoius personality choice
        PersonalityChoice oldChoice = selectedButton.PersonalityChoice;

        // If the choices are the same or if the choice is neutral, exit
        if (newChoice == oldChoice || newChoice == PersonalityChoice.NEUTRAL) return;

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

    }

    // Start is called before the first frame update
    void Start()
    {
        DialogButton.DialogButtonPressed += ChangeSelectedButtonIcon;
        allButtons = new List<DialogButton>();
        allButtons.Add(selectedButton);
        allButtons.AddRange(unselectedButtons);
        StartStory();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
