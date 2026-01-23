using System;
using UnityEngine;

namespace Lukomor.MVVM.Binders
{
#if UNITY_EDITOR
    [ExecuteInEditMode]
#endif
    public abstract class BinderDEPRECATED : MonoBehaviour
    {
        [SerializeField] private View _parentView;
        [SerializeField, HideInInspector] private string _viewModelTypeFullName; // view model type for binding
        [SerializeField, HideInInspector] private string _propertyName; // property name for binding
        [SerializeField, HideInInspector] private string _id;

        public string ViewModelTypeFullName => _viewModelTypeFullName;
        protected string PropertyName => _propertyName;

        private IDisposable _binding; // Runtime binding

        private void Start()
        {
            OnStart();
        }

        protected virtual void OnStart()
        {
        }

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

        protected virtual void OnDestroyed()
        {
        }

        public void Bind(IViewModel viewModel)
        {
            _binding = BindInternal(viewModel);
        }
        
        protected abstract IDisposable BindInternal(IViewModel viewModel);

        protected void OnValidate()
        {

        }

#if UNITY_EDITOR
        private void Reset()
        {
            if (string.IsNullOrEmpty(_id))
            {
                _id = Guid.NewGuid().ToString();
            }

            if (_parentView == null)
            {
                _parentView = GetComponentInParent<View>();
                _parentView.RegisterBinder(this);
            }
        }
#endif
    }
}