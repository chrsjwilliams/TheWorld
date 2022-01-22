using UnityEngine;
//using Sirenix.OdinInspector;
using System;

[CreateAssetMenu(fileName = "New Int Variable"
                , menuName = "Variable/Bool")]
public class BoolVariable : VariableSO
{
    [SerializeField] bool _value;
    public bool value
    {
        get
        {
            return _value;
        }
        set
        {
            // "value" here is the C# keyword for setters
            _value = value;
        }
    }
}
