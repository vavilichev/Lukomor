using System;
using System.Linq;
using Lukomor.MVVM.Editor;
using UnityEngine;

namespace Lukomor.MVVM.Binders
{
    public abstract class CommandBinderBase : MonoBehaviour
    {
        [SerializeField] private View _sourceView;
        [SerializeField] private string _viewModelCommandPropertyName;

        protected View SourceView => _sourceView;
        protected string ViewModelCommandPropertyName => _viewModelCommandPropertyName;
        protected IDisposable ViewModelSubscription;
        
        public abstract Type CommandType { get; }

        private void OnDestroy()
        {
            ViewModelSubscription.Dispose();
        }
        
        #if UNITY_EDITOR
        public void CheckValidation()
        {
            if (_sourceView == null)
            {
                DrawWarningIcon();
                return;
            }

            if (string.IsNullOrEmpty(_viewModelCommandPropertyName))
            {
                DrawWarningIcon();
                return;
            }
            
            RemoveWarningIcon();
        }

        private void DrawWarningIcon()
        {
            WarningIconDrawer.AddWarningView(gameObject.GetInstanceID());    
        }

        private void RemoveWarningIcon()
        {
            WarningIconDrawer.RemoveWarningView(gameObject.GetInstanceID());    
        }
        
        #endif
    }

    public abstract class CommandBinder : CommandBinderBase
    {
        private ICommand _command;
        
        public override Type CommandType { get; } = typeof(ICommand);

        protected virtual void Start()
        {
            ViewModelSubscription = SourceView.ViewModel.Subscribe(viewModel =>
            {
                if (viewModel == null)
                {
                    return;
                }

                _command = GetCommandFromViewModel(viewModel);
            });
        }

        public void ExecuteCommand()
        {
            if (_command == null)
            {
                Debug.LogError("Command is null.", gameObject);
                return;
            }
            
            _command.Execute();
        }
        
        private ICommand GetCommandFromViewModel(IViewModel viewModel)
        {
            var allViewModelProperties = viewModel.GetType().GetProperties();
            var requiredProperty = allViewModelProperties.FirstOrDefault(p => p.Name == ViewModelCommandPropertyName);
            
            if (requiredProperty == null)
            {
                Debug.LogError($"Couldn't find command in view model {viewModel.GetType().Name}. Property: {ViewModelCommandPropertyName}");
                return null;
            }
            
            var propertyType = requiredProperty.PropertyType;
            var isCommand = CommandType.IsAssignableFrom(propertyType);
            if (!isCommand)
            {
                Debug.LogError($"Found property is not a command. ViewModel ({viewModel.GetType().Name}), Property: {ViewModelCommandPropertyName}");
                return null;
            }

            var command = (ICommand)requiredProperty.GetValue(viewModel);
            return command;
        }
    }

    public abstract class CommandBinder<T> : CommandBinderBase
    {
        private ICommand<T> _command;
        
        public override Type CommandType { get; } = typeof(ICommand<T>);
        
        protected virtual void Start()
        {
            ViewModelSubscription = SourceView.ViewModel.Subscribe(viewModel =>
            {
                if (viewModel == null)
                {
                    return;
                }

                _command = GetCommandFromViewModel(viewModel);
            });
        }
        
        public void ExecuteCommand(T parameter)
        {
            if (_command == null)
            {
                Debug.LogError("Command is null.", gameObject);
                return;
            }
            
            _command.Execute(parameter); 
        }
        
        private ICommand<T> GetCommandFromViewModel(IViewModel viewModel)
        {
            var allViewModelProperties = viewModel.GetType().GetProperties();
            var requiredProperty = allViewModelProperties.FirstOrDefault(p => p.Name == ViewModelCommandPropertyName);
            
            if (requiredProperty == null)
            {
                Debug.LogError($"Couldn't find command in view model {viewModel.GetType().Name}. Property: {ViewModelCommandPropertyName}");
                return null;
            }
            
            var propertyType = requiredProperty.PropertyType;
            var isCommand = CommandType.IsAssignableFrom(propertyType);
            if (!isCommand)
            {
                Debug.LogError($"Found property is not a command. ViewModel ({viewModel.GetType().Name}), Property: {ViewModelCommandPropertyName}");
                return null;
            }

            var command = (ICommand<T>)requiredProperty.GetValue(viewModel);
            return command;
        }
    }
}