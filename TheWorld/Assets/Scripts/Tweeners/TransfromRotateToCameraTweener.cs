using UnityEngine;
using DG.Tweening;

public class TransfromRotateToCameraTweener : TransformRotateTweener
{
    [SerializeField] GameObjectCollectionSO cameraCollection;

    protected override Tweener LocalPlay()
    {
        if (cameraCollection.contents.Count < 1)
        {
            Debug.Log("Collection is empty for " + name + " is empty");
            return target.DORotate(Vector3.zero, duration, RotateMode.FastBeyond360);
        }

        var camera = cameraCollection.contents[0].GetComponent<Camera>();

        var targetRot = camera.transform.localRotation.eulerAngles;
        return target.DORotate(targetRot, duration, RotateMode.FastBeyond360).SetEase(easing);
    }
}
