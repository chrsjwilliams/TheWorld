//using Sirenix.OdinInspector;
using DG.Tweening;
using UnityEngine;

public class TransformPositionTweener : TransformTweener
{
    [SerializeField] bool snapping;
    [SerializeField] bool hasTarget;
    //[SerializeField, ShowIf("hasTarget", true)] GameObjectCollectionSO targetObject;
    [SerializeField] GameObjectCollectionSO targetObject;


    [SerializeField] Ease easing;

    protected override Tweener LocalPlay()
    {
        if(targetObject.contents.Count < 1)
        {
            Debug.Log("Collection is empty for " + name + " is empty");
            return target.DOLocalMove(Vector3.zero, duration, snapping);
        }

        var objPlane = new Plane(Camera.allCameras[0].transform.forward * -1, transform.position);

        Ray ray = Camera.allCameras[0].ScreenPointToRay(targetObject.contents[0].transform.position);
        float rayDistance;
        objPlane.Raycast(ray, out rayDistance);

        var targetPosition = ray.GetPoint(rayDistance);
        return target.DOLocalMove(targetPosition, duration, snapping).SetEase(easing);
    }
}
