using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegisterWithCollection : MonoBehaviour
{

    public List<GameObjectCollectionSO> collectionSOs;

    private void Awake()
    {
        foreach (var collection in collectionSOs)
        {
            collection.Add(gameObject);
        }
    }

    private void OnDestroy()
    {
        foreach (var collection in collectionSOs)
        {
            collection.Remove(gameObject);
        }
    }
}
