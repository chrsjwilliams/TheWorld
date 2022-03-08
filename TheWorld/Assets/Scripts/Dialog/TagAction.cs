using System;
using UnityEngine;
using Sirenix.OdinInspector;

public abstract class TagAction : ScriptableObject
{
    protected Action callback;
    [Button]
    public abstract void ExecuteAction();
}

[CreateAssetMenu(fileName = "New Animation Action"
                , menuName = "Dialog/Tag Actions/Animation Action")]
public class AnimationAction : TagAction
{
    public CharacterData character;
    [SerializeField]string animationName;
    public void Init(CharacterData _character, string _animationName)
    {
        character = _character;
        animationName = _animationName;
    }

    public override void ExecuteAction()
    {
        character.PlayAnimation(animationName);
    }
}

[CreateAssetMenu(fileName = "New SFX Action"
                , menuName = "Dialog/Tag Actions/SFX Action")]
public class PlaySFXAction : TagAction
{
    [SerializeField] SFX sfx;
    public void Init(string sfxName)
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

[CreateAssetMenu(fileName = "New BGM Action"
                , menuName = "Dialog/Tag Actions/BGM Action")]
public class PlayBGMAction : TagAction
{
    [SerializeField] BGM bgm;
    public void Init(string bgmName)
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