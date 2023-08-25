using System;
using UnityEngine;

namespace Lukomor.MVVM
{
    public class RootView : MonoBehaviour, IView
    {
        [SerializeField, HideInInspector] private string _viewModelPath;
        
        public Type ViewModelType
        {
            get
            {
                if (_viewModelType == null)
                {
                    _viewModelType = Type.GetType(_viewModelPath);
                }

                return _viewModelType;
            }
        }

        private Type _viewModelType;
    }
}