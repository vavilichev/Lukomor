using System;
using System.Reactive.Subjects;
using PlasticGui.Diff;
using UnityEngine;

namespace Lukomor.MVVM.Binders
{
    public abstract class ObservableBinder : MonoBehaviour
    {
        [SerializeField, HideInInspector] private BindingType _bindingType = BindingType.View;
        
        // For ViewModel
        [SerializeField, HideInInspector] private View _sourceView;
        [SerializeField, HideInInspector] private string _viewModelPropertyName;
        
        // For other binders
        [SerializeField, HideInInspector] private ObservableBinder _sourceBinder;
        
        protected IDisposable Subscription;
        
        protected View SourceView => _sourceView;
        protected ObservableBinder SourceBinder => _sourceBinder;
        protected string ViewModelPropertyName => _viewModelPropertyName;
        
        public BindingType BindingType => _bindingType;
        public abstract Type InputType { get; }
        public abstract Type OutputType { get; }

        protected virtual void OnDestroy()
        {
            Subscription?.Dispose();
        }
    }
    
    public abstract class ObservableBinder<TIn, TOut> :  ObservableBinder, IObservableStream<TOut>
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
            Subscription = inputStream?.Subscribe(value => {
                var result = HandleValue(value);
                _outputStream.OnNext(result);
            });
        }
    }
    
    public abstract class ObservableBinder<TInOut> : ObservableBinder<TInOut, TInOut>
    {
    }
}