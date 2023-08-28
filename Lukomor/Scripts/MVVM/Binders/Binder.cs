using System;
using UnityEngine;

namespace Lukomor.MVVM
{
    public abstract class Binder : MonoBehaviour
    {
        [SerializeField] private string _viewModelTypeFullName;
        [SerializeField] private string _propertyName;

        public string ViewModelTypeFullName => _viewModelTypeFullName;
        protected string PropertyName => _propertyName;

        private IDisposable _binding;

        private void OnDestroy()
        {
            _binding?.Dispose();
        }

        public void Bind(IViewModel viewModel)
        {
            _binding = BindInternal(viewModel);
        }

        protected abstract IDisposable BindInternal(IViewModel viewModel);
    }
}