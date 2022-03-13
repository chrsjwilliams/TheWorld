using System;
using UnityEngine;
using Sirenix.OdinInspector;

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