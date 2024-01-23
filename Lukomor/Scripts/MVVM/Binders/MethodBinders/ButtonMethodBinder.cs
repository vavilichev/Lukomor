using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace Lukomor.MVVM.Binders
{
    [RequireComponent(typeof(Button))]
    public class ButtonMethodBinder : EmptyMethodBinder
    {
        [SerializeField] private Button _button;
        
        private IViewModel _viewModel;
        private MethodInfo _cachedMethod;

        private void OnEnable()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                return;
            }
#endif
            _button.onClick.AddListener(OnClick);

        }

        private void OnDisable()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                return;
            }
#endif
            
            _button.onClick.RemoveListener(OnClick);
        }

        protected override IDisposable BindInternal(IViewModel viewModel)
        {
            _viewModel = viewModel;
            _cachedMethod = viewModel.GetType().GetMethod(MethodName);

            return base.BindInternal(viewModel);
        }

        private void OnClick()
        {
            _cachedMethod.Invoke(_viewModel, null);
        }

#if UNITY_EDITOR
        private void Reset()
        {
            if (_button == null)
            {
                _button = GetComponent<Button>();
            }
        }
#endif
    }
}