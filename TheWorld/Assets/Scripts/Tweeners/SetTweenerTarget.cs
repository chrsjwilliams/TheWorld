using UnityEngine;

public class SetTweenerTarget : MonoBehaviour
{


    [SerializeField] Transform target;

    private void Start()
    {
        foreach (var tweener in GetComponentsInChildren<TransformTweener>())
        {
            tweener.target = target;
        }

        var rectTransform = target.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            foreach (var tweener in GetComponentsInChildren<RectTransformTweener>())
            {
                tweener.target = rectTransform;
            }
        }
    }

}