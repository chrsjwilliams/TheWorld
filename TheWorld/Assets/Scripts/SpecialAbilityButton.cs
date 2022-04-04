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

    public void ShowAbilityDescription(bool show)
    {
        if(show)
        {
            fadeInTweener?.Play();
            List<DialogNode> nodes = dialogGraphParser.GetNodesForAbility(selectedCharacter.PersonalityChoice);
            nodeSelectionManager.PopulateOption(nodes);
        }
        else
        {
            fadeOutTweener?.Play();
        }
    }

    /*
     *  ~TODO: Figure out how to display x number of nodes
     * 
     */ 

    /* 
     * Ox       -   not sure yet
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

}
