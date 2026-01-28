using System;
using System.Linq;
using System.Reactive.Disposables;
using Lukomor.MVVM.Editor;
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
        
        #if UNITY_EDITOR
        public void CheckValidation()
        {
            // if (_bindingType == BindingType.View)
            // {
            //     if (_sourceView != null)
            //     {
            //         var sourceViewModelType =
            //             ViewModelsEditorUtility.ConvertViewModelType(_sourceView.ViewModelTypeFullName);
            //         if (sourceViewModelType == null)
            //         {
            //             _viewModelPropertyName = null;
            //             DrawWarningIcon();
            //             return;
            //         }
            //
            //         var allViewModelProperties = sourceViewModelType.GetProperties();
            //         var isPropertyExist = allViewModelProperties.Any(p => p.Name == _viewModelPropertyName);
            //
            //         if (isPropertyExist)
            //         {
            //             RemoveWarningIcon();
            //             return;
            //         }
            //
            //         DrawWarningIcon();
            //     }
            //     else
            //     {
            //         // no view reference
            //         DrawWarningIcon();
            //     }
            // }
            // else
            // {
            //     if (_sourceBinder != null)
            //     {
            //         RemoveWarningIcon();
            //     }
            //     else
            //     {
            //         // no binder selected
            //         DrawWarningIcon();
            //     }
            // }
        }
        
        private void DrawWarningIcon()
        {
            WarningIconDrawer.AddWarningView(gameObject.GetInstanceID());
        }

        private void RemoveWarningIcon()
        {
            WarningIconDrawer.RemoveWarningView(gameObject.GetInstanceID());
        }
        
        #endif
    }

    public abstract class ObservableCollectionBinder<TValue> : ObservableCollectionBinder
    {
        public override Type InputType => typeof(TValue);

        protected virtual void Start()
        {
            Subscriptions.Add(SourceView.ViewModel.Subscribe(viewModel =>
            {
                if (viewModel == null)
                {
                    return;
                }

                var inputStream = GetPropertyFromViewModel(viewModel);

                Subscriptions.Add(inputStream.Added.Subscribe(OnValueAdded));
                Subscriptions.Add(inputStream.Removed.Subscribe(OnValueRemoved));
            }));
        }

        private IReadOnlyReactiveCollection<TValue> GetPropertyFromViewModel(IViewModel sourceViewModel)
        {
            var vmType = sourceViewModel.GetType();
            var property = vmType.GetProperty(ViewModelPropertyName);
            var propertyValue = (IReadOnlyReactiveCollection<TValue>)property?.GetValue(sourceViewModel);
            return propertyValue;
        }
        
        protected abstract void OnValueAdded(TValue viewModel);
        protected abstract void OnValueRemoved(TValue viewModel);
    }
}