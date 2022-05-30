using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "New Dialog Graph Generator"
                , menuName = "Dialog/Cast List")]
[System.Serializable]
public class CastList : SerializedScriptableObject
{
    [SerializeField] List<CharacterData> characters;
    public List<CharacterData> Characters { get { return characters; } }

    [SerializeField] CharacterData player;
    public CharacterData Player { get { return player; } }

    [SerializeField] List<StoryCharacter> activeCharacters = new List<StoryCharacter>();

    public void ClearActiveCharactersModel()
    {
        activeCharacters.Clear();
    }

    public StoryCharacter GetCharacterModel(StoryCharacter data)
    {
        Debug.Log("DATA: " + data.characterName);
        foreach(StoryCharacter d in activeCharacters)
        {
            if (d.characterName == data.characterName)
                return d;
        }

        Debug.Log("Character " + data + " not found.");
        return null;
    }

    public void AddCharacterModel(StoryCharacter data)
    {
        if(!activeCharacters.Contains(data))
        {
            activeCharacters.Add(data);
        }
    }

    public void AddCharactersModel(List<StoryCharacter> data)
    {
        foreach (StoryCharacter d in data)
        {
            if (!activeCharacters.Contains(d))
            {
                activeCharacters.Add(d);
            }
        }
    }

    public void RemoveCharacterModel(StoryCharacter data)
    {
        if (activeCharacters.Contains(data))
        {
            activeCharacters.Remove(data);
        }
    }

    public void RemoveCharactersModel(List<StoryCharacter> data)
    {
        foreach (StoryCharacter d in data)
        {
            if (activeCharacters.Contains(d))
            {
                activeCharacters.Add(d);
            }
        }
    }

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
