using System;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "New Profile Picture Action"
                , menuName = "Dialog/Tag Actions/Profile Picture Action")]
public class ChangeProfilePictureAction : TagAction
{
    [SerializeField] CharacterData character;
    [SerializeField] Emote emotion;
    public void Init(CharacterData c, Emote e)
    {
        character = c;
        emotion = e;
    }

    public void Init(CharacterData c, string e)
    {
        character = c;
        emotion = GetEmote(e);
    }

    Emote GetEmote(string str)
    {
        foreach (Emote emote in Enum.GetValues(typeof(Emote)))
        {
            if (emote.ToString().ToUpper() == str.ToUpper())
            {
                return emote;
            }
        }
        return Emote.NEUTRAL;
    }

    public override void ExecuteAction()
    {
        character.SetProfilePicture(emotion);
    }
}