using UnityEngine;
using System.Collections.Generic;
//using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "GameObject Collection"
                , menuName = "GameObject Collection")]
public class GameObjectCollectionSO : ScriptableObject
{
    public List<GameObject> contents = new List<GameObject>();

    public int Count { get { return contents.Count; } }

    public void Add(GameObject gameObject)
    {
        contents.Add(gameObject);
    }

    public void Remove(GameObject gameObject)
    {
        contents.Remove(gameObject);
    }

}