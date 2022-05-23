using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using TMPro;
using System;

public class SpecialAbilityButton : SerializedMonoBehaviour
{
    [SerializeField]
    public struct CharacterAbility
    {
        public string title;
        [TextArea]
        public string description;
        public UnityEvent action;
    }
    [SerializeField] TextMeshProUGUI abilityTitle;
    [SerializeField] TextMeshProUGUI abilityDesctiption;
    [SerializeField] NodeSelectionManager nodeSelectionManager;
    [SerializeField] DialogGraphParser dialogGraphParser;
    [SerializeField] DialogNode selectedNode;
    [SerializeField] DialogButton selectedCharacter;
    [SerializeField] Dictionary<PersonalityChoice, CharacterAbility> abilityDictionary;

    [SerializeField] MonoTweener fadeInTweener;
    [SerializeField] MonoTweener fadeOutTweener;

    [SerializeField] TextMeshProUGUI messageBoxTitle;
    [SerializeField] TextMeshProUGUI messageBoxMessage;
    [SerializeField] MonoTweener messageBoxAppear;


    // Start is called before the first frame update
    void Start()
    {
        ShowAbilityDescription(false);
    }

    private void OnEnable()
    {
        NodeButton.NodeButtonPressed += OnNodeButtonPressed;
    }

    private void OnDisable()
    {
        NodeButton.NodeButtonPressed -= OnNodeButtonPressed;
    }

    private void OnNodeButtonPressed(NodeButton node)
    {
        selectedNode = dialogGraphParser.GetNode(node.NodeName);
    }

    public void ApplyAbility()
    {
        var personalityChoice = selectedCharacter.PersonalityChoice;
        if(abilityDictionary.ContainsKey(personalityChoice))
        {
            abilityDictionary[personalityChoice].action?.Invoke();
        }
        else
        {
            Debug.LogError("Choice " + personalityChoice.ToString() + " does not have a registered ability.");
        }
    }

    [SerializeField] List<DialogNode> nodes;

    public void ShowAbilityDescription(bool show)
    {
        if(show)
        {
            fadeInTweener?.Play();
            nodes = dialogGraphParser.GetNodesForAbility(selectedCharacter.PersonalityChoice);
            abilityTitle.text = abilityDictionary[selectedCharacter.PersonalityChoice].title;
            abilityDesctiption.text = abilityDictionary[selectedCharacter.PersonalityChoice].description;
            nodeSelectionManager.PopulateOption(nodes);
        }
        else
        {
            fadeOutTweener?.Play();
        }
    }

    /* 
     * Ox       -   see less desirable ends
     */
    public void OxAbility()
    {

    }

    /*
     * Eagle    -   can see the node where the special prize is hidden, 
     *              they also see which character’s node is active on 
     *              that - we never discussed about this before, I’ll 
     *              explain later
     *              
     *              How should this information appear? Node title? Node text?
     *              both?
     */
    public void EagleAbility()
    {
        messageBoxAppear.Play();
        var itemNode = dialogGraphParser.FindSpecialItem();

        string messageText;
        if(itemNode == null)
        {
            messageText = "No special items found in this story.";
        }
        else
        {
            string choice = itemNode.nodeType.ToString();
            string choiceMessage;
            if (choice.ToLower() == "neutral")
            {
                choiceMessage = "The nodepath is still hazy.";
            }
            else
            { 

            choiceMessage = StartsWithVowel(choice) ?
                                    "An " + choice + " choice will get you there" :
                                    "A " + choice + " choice will get you there";
            }
            messageText = "The special item is found in the " + itemNode.name + " node."+
                          "\n" + choiceMessage;
        }

        messageBoxTitle.text = "Eagle Ability";
        messageBoxMessage.text = messageText;
    }

    bool StartsWithVowel(string word)
    {
        return word[0] == 'a' || word[0] == 'e' || word[0] == 'i' ||
            word[0] == 'o' || word[0] == 'u';
    }

    /* 
     * Human    -   can see 3 nodes ahead without moving the story wheel.
     *              So the special abilities work only with the corresponding 
     *              character.Human can foresee human nodes only
     *              
     *              How are the nodes supposed to be viewed? Node titles?
     *              node text? How should the node text appear?
     */              
    public void HumanAbility()
    {

    }

    /*
     *          
     * Lion     -   can go back to any node
     */
    public void LionAbility()
    {
        dialogGraphParser.GoToVisitedNode(selectedNode);
    }

    // go back a set number of times
    // 3 times each story

}
