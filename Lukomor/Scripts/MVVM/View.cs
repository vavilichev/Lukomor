using System;
using System.Linq;
using System.Reactive.Disposables;
using Lukomor._Lukomor.Lukomor.Scripts.MVVM.Editor;
using Lukomor.Reactive;
using UnityEngine;

namespace Lukomor.MVVM
{
#if UNITY_EDITOR
    [ExecuteInEditMode]
#endif
    public class View : MonoBehaviour
    {
        [SerializeField] private View _sourceView;

        [SerializeField] private string _viewModelTypeFullName;
        [SerializeField] private string _viewModelPropertyName;

        private readonly ReactiveProperty<IViewModel> _viewModel = new();
        private readonly CompositeDisposable _subscriptions = new();
        
        public View SourceView => _sourceView;
        public string ViewModelTypeFullName => _viewModelTypeFullName;
        public string ViewModelPropertyName => _viewModelPropertyName;
        public IObservable<IViewModel> ViewModel => _viewModel;

        public void Bind(IViewModel viewModel)
        {
            _viewModel.Value = viewModel;
        }

        private void Start()
        {
            if (_sourceView != null)
            {
                _subscriptions.Add(_sourceView.ViewModel.Subscribe(sourceViewModel =>
                {
                    var sourceViewModelType = sourceViewModel.GetType();
                    var allProperties = sourceViewModelType.GetProperties();
                    var requiredProperty = allProperties.FirstOrDefault(p => p.Name == _viewModelPropertyName);
                    if (requiredProperty == null)
                    {
                        throw new
                            Exception($"Property {_viewModelPropertyName} not found in view model {sourceViewModelType.Name}");
                    }

                    var requiredViewModelPropertyValue =
                        (IObservable<IViewModel>)requiredProperty.GetValue(sourceViewModel);
                    _subscriptions.Add(requiredViewModelPropertyValue.Subscribe(viewModel =>
                                                                                        _viewModel.Value = viewModel));
                }));
            }
        }

        public void Destroy()
        {
            // TODO: make it more flexible
            Destroy(gameObject);
        }

        private void OnDestroy()
        {
            _subscriptions.Dispose();
        }

#if UNITY_EDITOR
        
        private void Reset()
        {
            if (_sourceView == null)
            {
                _sourceView = gameObject.GetComponentsInParent<View>().FirstOrDefault(v => !ReferenceEquals(v, this));
                Editor.MVVMValidator.RequestValidation();
            }
        }

        public void SmartReset()
        {
            // reset only property, saving the viewModelTypeFullName
            ResetPropertyName();
        }

        public void ResetPropertyName()
        {
            _viewModelPropertyName = null;
        }

        public void ResetViewModelFullTypeName()
        {
            _viewModelTypeFullName = null;
        }
        
        private void OnTransformParentChanged()
        {
            Editor.MVVMValidator.RequestValidation();
        }

#endif
    }
}