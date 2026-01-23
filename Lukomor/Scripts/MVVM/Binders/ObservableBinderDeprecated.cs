using System;

namespace Lukomor.MVVM.Binders
{
    public abstract class ObservableBinderDeprecated : BinderDEPRECATED
    {
        public abstract Type ArgumentType { get; }
    }

    public abstract class ObservableBinderDeprecated<T> : ObservableBinderDeprecated
    {
        private IObservable<T> _cachedObservable;
        
        public override Type ArgumentType => typeof(T);
        
        public IObservable<T> CachedObservable => _cachedObservable;

        protected sealed override IDisposable BindInternal(IViewModel viewModel)
        {
            var property = viewModel.GetType().GetProperty(PropertyName);
            _cachedObservable = (IObservable<T>)property.GetValue(viewModel);
            var subscription = _cachedObservable.Subscribe(OnPropertyChanged);
        
            return subscription;
        }

        protected abstract void OnPropertyChanged(T newValue);
    }
}