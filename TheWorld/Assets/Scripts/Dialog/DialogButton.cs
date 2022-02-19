using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogButton : MonoBehaviour
{
    public static event Action<PersonalityChoice> DialogButtonPressed;

    [SerializeField] PersonalityChoice personalityChoice;
    public PersonalityChoice PersonalityChoice { get { return personalityChoice; } }
    [SerializeField] SpriteLookupSO spriteLookUp;
    [SerializeField] Button button;
    [SerializeField] Image buttonIcon;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnSelected()
    {
        DialogButtonPressed?.Invoke(personalityChoice);   
    }

    public void SetPersonalityChoice(PersonalityChoice choice)
    {
        if (choice == PersonalityChoice.NEUTRAL) return;

        personalityChoice = choice;
        Sprite updatedSprite;
        spriteLookUp.TryGetSprite(choice.ToString().ToUpper(), out updatedSprite);
        buttonIcon.sprite = updatedSprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
