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
    [SerializeField] Animator animator;

    public Sprite GetCharacterProfile(Emote emote)
    {
        return characterProfiles[emote];
    }

    public void PlayAnimation(string animation)
    {
        animator.Play(animation);
    }
}


public enum Emote { NEUTRAL, HAPPY, SAD, ANGRY }