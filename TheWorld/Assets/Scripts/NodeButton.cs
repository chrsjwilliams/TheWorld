using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NodeButton : MonoBehaviour
{
    public static event Action<NodeButton> NodeButtonPressed;

    [SerializeField] TextMeshProUGUI nodeText;
    [SerializeField] Button nodeButton;
    string nodeName;
    public string NodeName { get { return nodeName; } }

    public void Init(string name, bool canVisit = true)
    {
        nodeName = name;
        nodeText.text = name;
        nodeButton.interactable = canVisit;
    }

    public void OnButtonPressed()
    {
        NodeButtonPressed?.Invoke(this);
    }
}
