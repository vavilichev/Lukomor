using System;

namespace Lukomor.MVVM
{
    public class EmptyMethodBinder : MethodBinder
    {
        private event Action _action;
        
        protected override IDisposable BindInternal(IViewModel viewModel)
        {
            _action = (Action)Delegate.CreateDelegate(typeof(Action), viewModel, MethodName);

            return null;
        }
    }
}