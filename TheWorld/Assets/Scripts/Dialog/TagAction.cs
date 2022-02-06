using System;
using UnityEngine;

public abstract class TagAction : MonoBehaviour
{
    protected Action callback;
    public abstract void ExecuteAction();
}

public class AnimationAction : TagAction
{
    public CharacterData character;
    string animationName;
    public AnimationAction(CharacterData _character, string _animationName)
    {
        character = _character;
        animationName = _animationName;
    }

    public override void ExecuteAction()
    {
        character.PlayAnimation(animationName);
    }
}

public class PlaySFXAction : TagAction
{
    SFX sfx;
    public PlaySFXAction(string sfxName)
    {
        sfx = GetSFX(sfxName);
    }

    SFX GetSFX(string name)
    {
        foreach (SFX effect in Enum.GetValues(typeof(SFX)))
        {
            if (name.ToUpper() == effect.ToString().ToUpper())
            {
                return effect;
            }
        }
        Debug.LogError("Sound Effect " + name + " not found in SFX enum list");
        return SFX.CLICK;
    }

    public override void ExecuteAction()
    {
        Services.AudioManager.PlayClip(sfx);
    }
}

public class PlayBGMAction : TagAction
{
    BGM bgm;
    public PlayBGMAction(string bgmName)
    {
        bgm = GetBGM(bgmName);
    }

    BGM GetBGM(string name)
    {
        foreach (BGM effect in Enum.GetValues(typeof(BGM)))
        {
            if (name.ToUpper() == effect.ToString().ToUpper())
            {
                return effect;
            }
        }
        Debug.LogError("Sound Effect " + name + " not found in SFX enum list");
        return BGM.SILENCE;
    }

    public override void ExecuteAction()
    {
        Services.AudioManager.PlayBGM(bgm);
    }
}

public class ChangeProfilePictureAction : TagAction
{
    CharacterData character;
    Emote emotion;
    public ChangeProfilePictureAction(CharacterData c, Emote e)
    {
        character = c;
        emotion = e;
    }

    public ChangeProfilePictureAction(CharacterData c, string e)
    {
        character = c;
        emotion = GetEmote(e);
    }

    Emote GetEmote(string str)
    {
        foreach(Emote emote in Enum.GetValues(typeof(Emote)))
        {
            if(emote.ToString().ToUpper() == str.ToUpper())
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