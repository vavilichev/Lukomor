using System;
using System.Reactive.Disposables;
using Lukomor.Reactive;
using UnityEngine;

namespace Lukomor.MVVM.Binders
{
    public abstract class ObservableCollectionBinder : MonoBehaviour
    {
        [SerializeField, HideInInspector] private View _sourceView;
        [SerializeField, HideInInspector] private string _viewModelPropertyName;

        protected CompositeDisposable Subscriptions { get; } = new();

        protected View SourceView => _sourceView;
        protected string ViewModelPropertyName => _viewModelPropertyName;

        public abstract Type InputType { get; }
        
        protected virtual void OnDestroy()
        {
            Subscriptions?.Dispose();
        }
    }

    public abstract class ObservableCollectionBinder<TValue> : ObservableCollectionBinder
    {
        public override Type InputType => typeof(TValue);

        protected void Start()
        {
            var inputStream = GetPropertyFromViewModel(SourceView.ViewModel);
            
            Subscriptions.Add(inputStream.Added.Subscribe(OnValueAdded));
            Subscriptions.Add(inputStream.Removed.Subscribe(OnValueRemoved));
        }
        
        private IReadOnlyReactiveCollection<TValue> GetPropertyFromViewModel(IViewModel sourceViewModel)
        {
            var vmType = sourceViewModel.GetType();
            var property = vmType.GetProperty(ViewModelPropertyName);
            var propertyValue = (IReadOnlyReactiveCollection<TValue>)property?.GetValue(sourceViewModel);
            return propertyValue;
        }
        
        protected abstract void OnValueAdded(TValue value);
        protected abstract void OnValueRemoved(TValue value);
    }
}