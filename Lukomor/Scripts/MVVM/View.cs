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
#if UNITY_EDITOR
            Editor.WarningIconDrawer.ClearGameObject(gameObject.GetInstanceID());
#endif
        }

#if UNITY_EDITOR
        
        private void Reset()
        {
            if (_parentView == null)
            {
                _parentView = this.FirstOrDefaultParentView();
                CheckValidation();
            }
        }

        public void SmartReset()
        {
            // reset onlyt property
            _viewModelPropertyName = null;
        }

        public void ResetPropertyName()
        {
            _viewModelPropertyName = null;
        }

        public void ResetViewModelFullTypeName()
        {
            _viewModelTypeFullName = null;
        }

        public void CheckValidation()
        {
            if (string.IsNullOrEmpty(_viewModelPropertyName) && _parentView != null)
            {
                Debug.LogWarning($"View setup is not valid. Missing ViewModel property name for object {gameObject.name}", gameObject);
                ShowWarningIcon();
                return;
            }
            
            if (string.IsNullOrEmpty(_viewModelTypeFullName))
            {
                Debug.LogWarning($"View setup is not valid. Missing ViewModel for object {gameObject.name}", gameObject);
                ShowWarningIcon();
                return;
            }

            RemoveWarningIcon();
        }
        
        private void OnTransformParentChanged()
        {
            CheckValidation();
        }

        private void ShowWarningIcon()
        {
            Editor.WarningIconDrawer.AddWarning(gameObject.GetInstanceID(), GetInstanceID());
        }

        private void RemoveWarningIcon()
        {
            Editor.WarningIconDrawer.RemoveWarning(gameObject.GetInstanceID(), GetInstanceID());
        }
#endif
    }
}