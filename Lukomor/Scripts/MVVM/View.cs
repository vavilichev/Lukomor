using System;
using System.Linq;
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
        [SerializeField] private bool _isParentView;

        private readonly ReactiveProperty<IViewModel> _viewModel = new();
        private readonly CompositeDisposable _subscriptions = new();
        
        public string ViewModelTypeFullName => _viewModelTypeFullName;
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

        private void OnDestroy()
        {
            _subscriptions.Dispose();
#if UNITY_EDITOR
            RemoveWarningIcon();
            _parentView.CheckValidation();
#endif
        }

#if UNITY_EDITOR
        
        private void OnEnable()
        {
            if (!UnityEditor.EditorApplication.isPlaying)
            {
                if (_parentView == null)
                {
                    _parentView = this.FirstOrDefaultParentView();
                    _parentView?.CheckValidation();
                }
            }
        }

        private void Reset()
        {
            if (_parentView == null)
            {
                _parentView = gameObject.GetComponentsInParent<View>().FirstOrDefault(v => !ReferenceEquals(v, this));
                CheckValidation();
            }
        }
#endif

        public void Destroy()
        {
            // TODO: make it more flexible
            Destroy(gameObject);
        }

#if UNITY_EDITOR
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
            Editor.WarningIconDrawer.AddWarningView(this.GetId());
        }

        private void RemoveWarningIcon()
        {
            Editor.WarningIconDrawer.RemoveWarningView(this.GetId());
        }
#endif
    }
}