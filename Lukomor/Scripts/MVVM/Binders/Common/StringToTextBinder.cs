using UnityEngine;
using UnityEngine.UI;

namespace Lukomor.MVVM.Binders
{
    [RequireComponent(typeof(Text))]
    public class StringToTextBinder : ObservableBinder<string>
    {
        [SerializeField] private Text _textField;

        protected override string HandleValue(string value)
        {
            _textField.text = value;
            return value;
        }

#if UNITY_EDITOR
        private void Reset()
        {
            if (_textField == null)
            {
                _textField = GetComponent<Text>();
            }
        }
#endif
    }
}