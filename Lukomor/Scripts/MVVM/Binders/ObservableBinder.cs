using System;

namespace Lukomor.MVVM.Binders
{
    public abstract class ObservableBinder : Binder
    {
        public abstract Type ArgumentType { get; }
    }

    public abstract class ObservableBinder<T> : ObservableBinder
    {
        public override Type ArgumentType => typeof(T);

        protected sealed override IDisposable BindInternal(IViewModel viewModel)
        {
            var property = viewModel.GetType().GetProperty(PropertyName);
            var observable = (IObservable<T>)property.GetValue(viewModel);
            var subscription = observable.Subscribe(OnPropertyChanged);
        
            return subscription;
        }

        protected abstract void OnPropertyChanged(T newValue);
    }
}