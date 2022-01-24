using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "New Character"
                , menuName = "Dialog/Character")]
public class CharacterData : SerializedScriptableObject
{
    [SerializeField] public string characterName;
    [SerializeField] Dictionary<Emote, Sprite> characterProfiles;

    public Sprite GetCharacterProfile(Emote emote)
    {
        return characterProfiles[emote];
    }
}
