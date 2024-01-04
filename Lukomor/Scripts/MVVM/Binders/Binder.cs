using System;
using UnityEngine;

namespace Lukomor.MVVM.Binders
{
#if UNITY_EDITOR
    [ExecuteInEditMode]
#endif
    public abstract class Binder : MonoBehaviour
    {
        [SerializeField, HideInInspector] private string _viewModelTypeFullName;
        [SerializeField, HideInInspector] private string _propertyName;

        public string ViewModelTypeFullName => _viewModelTypeFullName;
        protected string PropertyName => _propertyName;

        private IDisposable _binding;

        private void Start()
        {
#if UNITY_EDITOR
            var parentView = GetComponentInParent<View>();
            parentView.RegisterBinder(this);
#endif
            
            OnStart();
        }

        protected virtual void OnStart() { }

        private void OnDestroy()
        {
            _binding?.Dispose();
            
#if UNITY_EDITOR
            var parentView = GetComponentInParent<View>();
            if (parentView)
            {
                parentView.RemoveBinder(this);
            }
#endif
            
            OnDestroyed();
        }

        protected virtual void OnDestroyed() { }

        public void Bind(IViewModel viewModel)
        {
            _binding = BindInternal(viewModel);
        }

        protected abstract IDisposable BindInternal(IViewModel viewModel);
    }
}