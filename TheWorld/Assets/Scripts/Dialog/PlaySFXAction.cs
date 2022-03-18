using System;
using UnityEngine;
using Sirenix.OdinInspector;

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
        //TODO: Find Sound Effects
        //Services.AudioManager.PlayClip(sfx);
    }
}