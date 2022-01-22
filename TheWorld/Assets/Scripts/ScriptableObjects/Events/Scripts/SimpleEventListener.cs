using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SimpleEventListener : MonoBehaviour
{

    [SerializeField] SimpleEvent simpleEvent;
    [SerializeField] UnityEvent onEvent;

    private void Awake()
    {
        simpleEvent.Subscribe(OnRaise);
    }

    private void OnDestroy()
    {
        simpleEvent.Unsubscribe(OnRaise);
    }

    void OnRaise()
    {
        onEvent?.Invoke();
    }
}
