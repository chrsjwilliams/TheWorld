using System.Collections.Generic;
//using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Sprite Lookup"
                , menuName = "Data/Sprite Lookup")]
public class SpriteLookupSO : ScriptableObject
{

    public Sprite fallbackSprite;
    public Dictionary<string, Sprite> sprites = new Dictionary<string, Sprite>();

    public bool TryGetSprite(string key, out Sprite sprite)
    {
        if (sprites.TryGetValue(key, out sprite))
        {
            return true;
        }
        else
        {
            sprite = fallbackSprite;
            return false;
        }
    }

}
