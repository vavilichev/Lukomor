using UnityEngine;

namespace Lukomor.MVVM.Binders
{
    public class BoolToColorBinder : ObservableBinder<bool, Color>
    {
        [SerializeField] private Color _trueColor = Color.green;
        [SerializeField] private Color _falseColor = Color.red;
        
        protected override Color HandleValue(bool value)
        {
            var result = value ? _trueColor : _falseColor;
            return result;
        }
    }
}