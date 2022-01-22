using UnityEngine;
//using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "New Int Variable"
                , menuName = "Variable/Int")]
public class IntVariable : VariableSO
{
    [SerializeField] int _value;
    public int value
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