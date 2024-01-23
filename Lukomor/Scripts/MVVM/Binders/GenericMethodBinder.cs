using System;

namespace Lukomor.MVVM.Binders
{
    public abstract class GenericMethodBinder : MethodBinder
    {
        public abstract Type ParameterType { get; }
    }

    public class GenericMethodBinder<T> : GenericMethodBinder
    {
        public override Type ParameterType => typeof(T);
        
        private event Action<T> _action;
        
        protected override IDisposable BindInternal(IViewModel viewModel)
        {
            _action = (Action<T>)Delegate.CreateDelegate(typeof(Action<T>), viewModel, MethodName);

            return null;
        }

        public void Perform(T value)
        {
            _action?.Invoke(value);
        }
    }
}