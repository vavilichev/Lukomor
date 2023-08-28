using System;

namespace Lukomor.MVVM
{
    public abstract class GenericMethodBinder : MethodBinder { }

    public class GenericMethodBinder<T> : GenericMethodBinder
    {
        private event Action<T> _action;
        
        protected override IDisposable BindInternal(IViewModel viewModel)
        {
            _action = (Action<T>)Delegate.CreateDelegate(typeof(Action<T>), viewModel, MethodName);

            return null;
        }
    }
}