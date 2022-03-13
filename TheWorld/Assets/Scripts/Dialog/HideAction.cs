using System;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "New Animation Action"
                , menuName = "Dialog/Tag Actions/Hide Action")]

public class HideAction : TagAction
{
    public CharacterData character;
    public void Init(CharacterData _character)
    {
        character = _character;
    }

    public void SetCharacterData(CharacterData data)
    {
        character = data;
    }

    public override void ExecuteAction()
    {
        character.Model.ShowCharacter(false);
    }
}
