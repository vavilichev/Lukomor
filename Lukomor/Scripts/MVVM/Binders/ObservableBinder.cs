using System;
using System.Linq;
using System.Reactive.Subjects;
using Lukomor.MVVM.Editor;
using UnityEngine;

namespace Lukomor.MVVM.Binders
{
    public abstract class ObservableBinder : BinderBase
    {
        [SerializeField] private BindingType _bindingType = BindingType.View;
        
        // For ViewModel
        [SerializeField] private string _viewModelPropertyName;
        
        // For other binders
        [SerializeField] private ObservableBinder _sourceBinder;
        
        public ObservableBinder SourceBinder => _sourceBinder;
        protected string ViewModelPropertyName => _viewModelPropertyName;
        
        public BindingType BindingType => _bindingType;
        public abstract Type InputType { get; }
        public abstract Type OutputType { get; }

        #if UNITY_EDITOR
        public override bool IsBroken()
        {
            if (_bindingType == BindingType.Binder)
            {
                return SourceBinder == null;
            }
            
            if (IsBrokenBasic(_viewModelPropertyName, out var sourceViewModelType))
            {
                return true;
            }
            
            var isBroken = IsBrokenViewModelProperty(sourceViewModelType);

            return isBroken;
        }

        public override void SmartReset()
        {
            if (_bindingType == BindingType.Binder)
            {
                return;
            }
            
            _viewModelPropertyName = null;
        }
        
        protected abstract bool IsBrokenViewModelProperty(Type sourceViewModelType);
        
        #endif
    }

    public abstract class ObservableBinder<TIn, TOut> : ObservableBinder, IObservableStream<TOut>
    {
        private readonly BehaviorSubject<TOut> _outputStream = new(default);
        private IDisposable _viewModelSubscription;
        public IObservable<TOut> OutputStream => _outputStream;
        public override Type InputType { get; } = typeof(TIn);
        public override Type OutputType { get; } = typeof(TOut);

        protected void Start()
        {
            if (SourceView != null)
            {
                _viewModelSubscription = SourceView.ViewModel.Subscribe(viewModel =>
                {
                    if (viewModel == null)
                    {
                        return;
                    }

                    var inputStream = GetPropertyFromViewModel(viewModel);
                    SubscribeOnInputStream(inputStream);
                });
            }
            else
            {
                var inputStream = GetPropertyFromOtherBinder(SourceBinder);
                SubscribeOnInputStream(inputStream);
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _viewModelSubscription?.Dispose();
        }

        protected abstract TOut HandleValue(TIn value);

        private IObservable<TIn> GetPropertyFromViewModel(IViewModel viewModel)
        {
            var vmType = viewModel.GetType();
            var property = vmType.GetProperty(ViewModelPropertyName);
            var propertyValue = (IObservable<TIn>)property?.GetValue(viewModel);
            return propertyValue;
        }

        private IObservable<TIn> GetPropertyFromOtherBinder(ObservableBinder otherBinder)
        {
            return ((ObservableBinder<TIn>)otherBinder).OutputStream;
        }

        private void SubscribeOnInputStream(IObservable<TIn> inputStream)
        {
            if (inputStream == null)
            {
                return;
            }

            Subscriptions.Add(inputStream.Subscribe(value =>
            {
                var result = HandleValue(value);
                _outputStream.OnNext(result);
            }));
        }

#if UNITY_EDITOR
        protected override bool IsBrokenViewModelProperty(Type sourceViewModelType)
        {
            var requiredPropertyType = typeof(IObservable<TIn>);
            var allViewModelProperties = sourceViewModelType.GetProperties();
            var allValidViewModelProperties =
                ViewModelsEditorUtility.FilterValidProperties(allViewModelProperties, requiredPropertyType);
            var doesRequiredPropertyExist = allValidViewModelProperties.Any(p => p.Name == ViewModelPropertyName);
            var isBroken = !doesRequiredPropertyExist;

            if (isBroken)
            {
                SmartReset();
            }

            return isBroken;
        }
#endif
    }

    public abstract class ObservableBinder<TInOut> : ObservableBinder<TInOut, TInOut>
    {
    }
}