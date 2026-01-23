using System;
using System.Reactive.Disposables;
using Lukomor.Reactive;
using UnityEngine;

namespace Lukomor.MVVM.Binders
{
    public abstract class ObservableViewModelsListBinder : MonoBehaviour
    {
        // For ViewModel
        [SerializeField, HideInInspector] private View _sourceView;
        [SerializeField, HideInInspector] private string _viewModelPropertyName;

        private CompositeDisposable _subscriptions;
        
        protected CompositeDisposable Subscriptions => _subscriptions;
        
        protected View SourceView => _sourceView;
        protected string ViewModelPropertyName => _viewModelPropertyName;
        
        protected void Start()
        {
            var inputStream = GetPropertyFromViewModel(SourceView.ViewModel);
            
            Subscriptions.Add(inputStream.Added.Subscribe(OnViewModelAdded));
            Subscriptions.Add(inputStream.Removed.Subscribe(OnViewModelRemoved));
        }
        
        protected virtual void OnDestroy()
        {
            Subscriptions?.Dispose();
        }
        
        private IReadOnlyReactiveCollection<IViewModel> GetPropertyFromViewModel(IViewModel sourceViewModel)
        {
            var vmType = sourceViewModel.GetType();
            var property = vmType.GetProperty(ViewModelPropertyName);
            var propertyValue = (IReadOnlyReactiveCollection<IViewModel>)property?.GetValue(sourceViewModel);
            return propertyValue;
        }
        
        protected abstract void OnViewModelAdded(IViewModel viewModel);
        protected abstract void OnViewModelRemoved(IViewModel viewModel);
    }
}