using System;
using System.Linq;
using Lukomor.MVVM.Editor;
using Lukomor.Reactive;
using UnityEngine;

namespace Lukomor.MVVM.Binders
{
    public abstract class ObservableCollectionBinder : BinderBase
    {
        [SerializeField, HideInInspector] private string _viewModelPropertyName;

        protected string ViewModelPropertyName => _viewModelPropertyName;

        public abstract Type InputType { get; }
        
        #if UNITY_EDITOR

        public override bool IsBroken()
        {
            if (IsBrokenBasic(_viewModelPropertyName, out var sourceViewModelType))
            {
                return true;
            }

            var isBroken = IsBrokenViewModelProperty(sourceViewModelType);
            return isBroken;
        }

        public override void SmartReset()
        {
            _viewModelPropertyName = null;
        }

        protected abstract bool IsBrokenViewModelProperty(Type sourceViewModelType);

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
        
        #if UNITY_EDITOR
        protected override bool IsBrokenViewModelProperty(Type sourceViewModelType)
        {
            var propertyType = typeof(IReadOnlyReactiveCollection<TValue>);
            var allViewModelProperties = sourceViewModelType.GetProperties();
            var allValidViewModelProperties =
                ViewModelsEditorUtility.FilterValidProperties(allViewModelProperties, propertyType);
            var doesRequiredPropertyExist = allValidViewModelProperties.Any(p => p.Name == ViewModelPropertyName);
            var isBroken = !doesRequiredPropertyExist;

            return isBroken;
        }

        #endif
    }
}