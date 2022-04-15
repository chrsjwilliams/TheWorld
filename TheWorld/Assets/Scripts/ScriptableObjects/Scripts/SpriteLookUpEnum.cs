using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Sprite Lookup Enum"
                , menuName = "Data/Sprite Lookup Enum")]
public class SpriteLookUpEnum : SerializedScriptableObject
{
    [SerializeField] public Sprite fallbackSprite;
    [SerializeField] public Dictionary<NODE_PICTURE, Sprite> sprites;

    public bool TryGetSprite(NODE_PICTURE key, out Sprite sprite)
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
