using UnityEngine;

public abstract class TransformTweener : MonoTweener
{

    [Tooltip("If this field is not filled, instead uses the Transform on this GameObject")]
    [SerializeField] Transform transformToUse;

    public Transform target
    {
        get
        {
            if (!transformToUse)
            {
                return transform;
            }
            return transformToUse;
        }
        set
        {
            transformToUse = value;
        }
    }

}