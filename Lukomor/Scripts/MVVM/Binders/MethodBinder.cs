namespace Lukomor.MVVM
{
    public abstract class MethodBinder : Binder
    {
        protected string MethodName => PropertyName;
    }
}