using UnityEngine;
using UnityEngine.UI;

namespace Lukomor.MVVM.Binders
{
    public class ColorToImageBinder : ObservableBinder<Color>
    {
        [SerializeField] private Image _img;
        protected override Color HandleValue(Color value)
        {
            _img.color = value;
            return value;
        }
    }
}