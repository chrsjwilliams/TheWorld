using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "New Character"
                , menuName = "Dialog/Character")]
public class CharacterData : SerializedScriptableObject
{
    [SerializeField] bool isPlayer;
    [SerializeField] public bool IsPlayer { get { return isPlayer; } }
    [SerializeField] bool isNarrator;
    [SerializeField] public bool IsNarrator { get { return isNarrator; } }
    [SerializeField] public string characterName;
    [SerializeField] Sprite currentProfilePicture;
    [SerializeField] Dictionary<Emote, Sprite> characterProfiles;
    [SerializeField] StoryCharacter model;
    [SerializeField] public StoryCharacter Model { get { return model; } }

    public Sprite GetCharacterProfile(Emote emote)
    {
        return characterProfiles[emote];
    }

    public void PlayAnimation(string animation)
    {
        Model.PlayAnimation(animation);
    }

    public void SetProfilePicture(Emote emote)
    {
        currentProfilePicture = GetCharacterProfile(emote);
    }
}


public enum Emote { NEUTRAL, HAPPY, SAD, ANGRY }