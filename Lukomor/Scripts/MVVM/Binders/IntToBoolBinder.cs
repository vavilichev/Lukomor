namespace Lukomor.MVVM.Binders
{
    public class IntToBoolBinder : ObservableBinder<int, bool>
    {
        protected override bool HandleValue(int value)
        {
            return value > 1;
        }
    }
}