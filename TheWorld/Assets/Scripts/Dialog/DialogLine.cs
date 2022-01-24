using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public class DialogLine
{
    
    public CharacterData speaker;
    public Emote emotion;
    [TextArea]
    public string line;
    public bool playerChoice;
    [Space(50)]
    [SerializeField, TextArea]
    string Notes;
}

public enum Emote { NUETRAL, HAPPY, SAD, ANGRY, CONFUSED}