using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ToggleButtonUsingBoolSO : MonoBehaviour
{
    [Header("Should we toggle based on the negation (i.e. BoolSO is true, but we assign false)")]
    [SerializeField] bool negation;
    [SerializeField] BoolVariable boolSO;
    [SerializeField] Button button;
    public bool turnOffButton;

    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        if (turnOffButton)
        {
            button.interactable = false;
            return;
        }

        button.interactable = negation ? !boolSO.value : boolSO.value;
    }
}
