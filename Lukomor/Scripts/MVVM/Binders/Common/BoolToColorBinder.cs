using UnityEngine;

namespace Lukomor.MVVM.Binders
{
    public class BoolToColorBinder : ObservableBinder<bool, Color>
    {
        protected override Color HandleValue(bool value)
        {
            var result = value ? Color.white : Color.black;
            return result;
        }
    }
}