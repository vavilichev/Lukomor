using System;
using System.Reactive.Disposables;
using Lukomor.Reactive;
using UnityEditorInternal;

namespace Lukomor.MVVM
{
    public abstract class ObservableBinder : Binder
    {
        public abstract Type ArgumentType { get; }
    }

    public abstract class ObservableBinder<T> : ObservableBinder
    {
        public override Type ArgumentType => typeof(T);

        protected IDisposable BindObservable(string propertyName, IViewModel viewModel, Action<T> callback)
        {
            var property = viewModel.GetType().GetProperty(propertyName);
            var observable = (IObservable<T>)property.GetValue(viewModel);
            var subscription = observable.Subscribe(callback);
        
            return subscription;
        }
        
        protected IDisposable BindCollection(string propertyName, IViewModel viewModel, Action<T> addedCallback, Action<T> removedCallback)
        {
            var propertyInfo = viewModel.GetType().GetProperty(propertyName);
            var reactiveCollection = (IReactiveCollection<T>)propertyInfo.GetValue(viewModel);
        
            var addedSubscription = reactiveCollection.Added.Subscribe(addedCallback);
            var removedSubscription = reactiveCollection.Removed.Subscribe(removedCallback);
            var compositeDisposable = new CompositeDisposable();
            
            compositeDisposable.Add(addedSubscription);
            compositeDisposable.Add(removedSubscription);
        
            return compositeDisposable;
        }
    }
}