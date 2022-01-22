using UnityEngine;

public abstract class RectTransformTweener : MonoTweener
{
    [Tooltip("If this field is not filled, instead uses the RectTransform on this GameObject")]
    [SerializeField] RectTransform rectToUse;

    // maintain a reference to the fallback RectTransform so that we aren't
    // running a GetComponent() call every time this property is accessed
    RectTransform cachedRect;
    public RectTransform target
    {
        get
        {
            if (!rectToUse)
            {
                if (!cachedRect)
                {
                    cachedRect = GetComponent<RectTransform>();
                }
                return cachedRect;
            }
            return rectToUse;
        }
        set
        {
            rectToUse = value;
        }
    }

}