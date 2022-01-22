using UnityEngine;

[CreateAssetMenu(fileName = "New Float Variable"
                , menuName = "Variable/Float")]
public class FloatVariable : VariableSO
{
    [SerializeField] float _value;
    public float value
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