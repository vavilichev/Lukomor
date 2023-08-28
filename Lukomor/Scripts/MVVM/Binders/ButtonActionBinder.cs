﻿using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace Lukomor.MVVM
{
    [RequireComponent(typeof(Button))]
    public class ButtonActionBinder : EmptyMethodBinder
    {
        [SerializeField] private Button _button;
        [SerializeField] private string _methodName;

        private IViewModel _viewModel;
        private MethodInfo _cachedMethod;

        private void OnEnable()
        {
            _button.onClick.AddListener(OnClick);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnClick);
        }

        public Type ViewModelType { get; }

        public void Bind(IViewModel viewModel)
        {
            _viewModel = viewModel;
            _cachedMethod = viewModel.GetType().GetMethod(_methodName);
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