using UnityEngine;
using UnityEngine.UI;

namespace Lukomor.MVVM.Binders
{
    public class ColorToTextBinder : ObservableBinder<Color>
    {
        [SerializeField] private Text _textField;

        protected override Color HandleValue(Color value)
        {
            _textField.color = value;
            return value;
        }

#if UNITY_EDITOR
        private void Reset()
        {
            _textField = GetComponent<Text>();
        }
#endif
    }
}