using System;
using System.Reactive.Disposables;
using Lukomor.Reactive;
using UnityEngine;

namespace Lukomor.MVVM
{
#if UNITY_EDITOR
    [ExecuteInEditMode]
#endif
    public class View : MonoBehaviour
    {
        [SerializeField] private View _parentView;

        [SerializeField] private string _viewModelTypeFullName;
        [SerializeField] private string _viewModelPropertyName;

        private readonly ReactiveProperty<IViewModel> _viewModel = new();
        private readonly CompositeDisposable _subscriptions = new();
        
        public View ParentView => _parentView;
        public string ViewModelTypeFullName => _viewModelTypeFullName;
        public string ViewModelPropertyName => _viewModelPropertyName;
        public IObservable<IViewModel> ViewModel => _viewModel;

        public void Bind(IViewModel viewModel)
        {
            _viewModel.Value = viewModel;
        }
        
        private void Start()
        {
            if (_parentView != null)
            {
                _subscriptions.Add(_parentView.ViewModel.Subscribe(viewModel => { _viewModel.Value = viewModel; }));
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
            if (_parentView == null)
            {
                _parentView = this.FirstOrDefaultParentView();
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