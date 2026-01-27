using UnityEngine;
using UnityEngine.Events;

namespace Lukomor.MVVM.Binders
{
    public class BoolToColorUnityEventBinder : ObservableBinder<bool, Color>
    {
        [SerializeField] private Color _colorTrue = Color.white;
        [SerializeField] private Color _colorFalse = Color.white;

        [SerializeField] private UnityEvent<Color> _event;

        protected override Color HandleValue(bool value)
        {
            var color = value ? _colorTrue : _colorFalse;
            
            _event.Invoke(color);

            return color;
        }
    }
}