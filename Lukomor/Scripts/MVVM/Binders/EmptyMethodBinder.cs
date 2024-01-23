using System;

namespace Lukomor.MVVM.Binders
{
    public class EmptyMethodBinder : MethodBinder
    {
        private event Action _action;
        
        protected override IDisposable BindInternal(IViewModel viewModel)
        {
            _action = (Action)Delegate.CreateDelegate(typeof(Action), viewModel, MethodName);

            return null;
        }

        public void Perform()
        {
            _action?.Invoke();
        }
    }
}