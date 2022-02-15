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

    int dialogLineIndex = 0;
    bool madeNodeSelection = false;


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

    public void PreviousButtonPRessed()
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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
