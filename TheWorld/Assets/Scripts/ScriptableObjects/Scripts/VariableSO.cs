using UnityEngine;
//using Sirenix.OdinInspector;

public abstract class VariableSO : ScriptableObject
{
    [TextArea]
    public string notes = "Describe what this is for (for Inspector display only)";

    //[BoxGroup("Display"), Tooltip("Name for displaying in the UI")]
    public string displayName;
    //[BoxGroup("Display"), Tooltip("The icon for the store, HUD, and Inventory UI")]
    public Sprite icon;

    [SerializeField] protected SimpleEvent OnValueChanged;

}