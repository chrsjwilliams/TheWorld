using UnityEngine;
//using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "New String Variable"
                , menuName = "Variable/String")]
public class StringVariable : VariableSO
{
    [SerializeField] string _value;
    public string value
    {
        get
        {
            return _value;
        }
        set
        {
            var valueChanged = _value != value ? true : false;
            // "value" here is the C# keyword for setters
            _value = value;

            if(valueChanged)
                OnValueChanged.Raise();
        }
    }
}