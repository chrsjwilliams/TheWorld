using System;
using UnityEngine;
using Sirenix.OdinInspector;


/*
 * 
 *      TODO:   -   passing animation action from scriptable objects to models
 *              -   have animation tags remember their animation
 * 
 * 
 */ 

[CreateAssetMenu(fileName = "New Animation Action"
                , menuName = "Dialog/Tag Actions/Animation Action")]
public class AnimationAction : TagAction
{
    public CharacterData character;
    [SerializeField] string animationName;
    public void Init(CharacterData _character, string _animationName)
    {
        character = _character;
        animationName = _animationName;
    }

    public void SetCharacterData(CharacterData data)
    {
        character = data;
    }

    public override void ExecuteAction()
    {
        var model = Services.CastList.GetCharacterModel(character.Model);
        if(model == null)
        {
            Debug.LogError("Character " + character.characterName + " not found in selected cast list");
            return;
        }
        Debug.Log("Trying to play animation " + animationName);
        model.PlayAnimation(animationName);
    }
}