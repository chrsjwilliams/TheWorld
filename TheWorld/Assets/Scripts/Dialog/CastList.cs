using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class CastList : SerializedScriptableObject
{
    [SerializeField] List<CharacterData> characters;
    public List<CharacterData> Characters { get { return characters; } }

    public bool Contians(string name)
    {
        foreach(CharacterData charatcer in characters)
        {
            if (charatcer.characterName == name) return true;
        }

        return false;
    }

    public CharacterData GetCharacter(string name)
    {
        foreach (CharacterData charatcer in characters)
        {
            if (charatcer.characterName.ToLower() == name.ToLower()) return charatcer;
        }

        return null;
    }

    public CharacterData GetCharacter(CharacterData data)
    {
        foreach (CharacterData charatcer in characters)
        {
            if (charatcer == data) return charatcer;
        }

        return null;
    }

}
