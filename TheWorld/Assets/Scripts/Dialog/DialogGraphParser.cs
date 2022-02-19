using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class DialogGraphParser : MonoBehaviour
{
    [SerializeField] DialogGraph dialogGraph;

    [SerializeField] DialogNode currentNode;

    [SerializeField] Stack<DialogNode> visitedNodes;

    [SerializeField] TextMeshProUGUI dialogText;

    [SerializeField] Image playerProfile;
    [SerializeField] Image npcProfile;

    [SerializeField] CanvasGroup playerProfileCanvasGroup;
    [SerializeField] CanvasGroup npcProfileCanvasGroup;


    [SerializeField] PersonalityChoice currentDialogChoice;
    [SerializeField] DialogButton selectedButton;
    [SerializeField] List<DialogButton> unselectedButtons;


    int dialogLineIndex = 0;
    bool madeNodeSelection = false;


    // Choices we have not selected
    // last choice selected

    public void NextButtonPressed()
    {
        if (dialogLineIndex == currentNode.speakingLines.Count - 1)
        {
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

    public void StartStory()
    {
        if (currentNode != null)
        {
            Debug.LogError("Current Node is not empty");
            return;
        }


        currentNode = dialogGraph.startingNode;

        visitedNodes.Push(currentNode);
        currentNode.ExecuteTagActions(() => { });
        dialogLineIndex = 0;
        DisplayNextDialogLine();

    }

    void MoveToNextNode()
    {
        if (!madeNodeSelection) return;
        
        if (currentNode.HasPersonalityChoice(currentDialogChoice))
        {
            //  Set current node to next node
            currentNode = currentNode.nextNodes[currentDialogChoice];

            //  add new node to node stack
            visitedNodes.Push(currentNode);
            // execute actions on the node
            currentNode.ExecuteTagActions(() => { });
            //  reset dialog line index
            dialogLineIndex = 0;
            //  display dialog line
            DisplayNextDialogLine();
        }
        else
        {
            Debug.LogError("Choice " + currentDialogChoice.ToString() +
                " not avialable for " + currentNode.name + " node");
        }
    }

    void DisplayNextDialogLine()
    {

        DialogLine currentLine = currentNode.speakingLines[dialogLineIndex];

        CharacterData character = currentLine.speaker;

        if (character == Services.CastList.Player)
        {
            playerProfileCanvasGroup.alpha = 1;
            npcProfileCanvasGroup.alpha = 0;
            playerProfile.sprite = character.GetCharacterProfile(Emote.NEUTRAL);
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

    bool AtLastLineOfDialog()
    {
        return dialogLineIndex == currentNode.speakingLines.Count - 1;
    }

    void ChangeSelectedButtonIcon(PersonalityChoice newChoice)
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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
