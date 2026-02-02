using UnityEngine;
using UnityEngine.UI;

namespace Lukomor.MVVM.Binders
{
    public class BoolToTextBinder : ObservableBinder<bool, string>
    {
        [SerializeField] private Text _textField;
        [SerializeField] private string _trueValue;
        [SerializeField] private string _falseValue;
        protected override string HandleValue(bool value)
        {
            var text = value ? _trueValue : _falseValue;
            _textField.text = text;
            return text;
        }
    }
}