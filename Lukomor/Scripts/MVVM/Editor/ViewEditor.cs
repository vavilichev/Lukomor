using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Lukomor.MVVM.Editor
{
    [CustomEditor(typeof(View))]
    public class ViewEditor : UnityEditor.Editor
    {
        private SerializedProperty _viewModelTypeFullName;
        private SerializedProperty _viewModelPropertyName;
        private SerializedProperty _isParentView;
        private SerializedProperty _childBinders;
        private SerializedProperty _subViews;
        private SerializedProperty _showEditorLogs;
        private View _view;        private SerializedProperty _parentView;
        private readonly Dictionary<string, string> _viewModelNames = new();
        
        // help 
        private readonly List<View> _parentViews = new();
        private readonly List<string> _viewModelPropertyNames = new();

        private void OnEnable()
        {
            _view = (View)target;
            _viewModelTypeFullName = serializedObject.FindProperty(nameof(_viewModelTypeFullName));
            _viewModelPropertyName = serializedObject.FindProperty(nameof(_viewModelPropertyName));
            _isParentView = serializedObject.FindProperty(nameof(_isParentView));
            _childBinders = serializedObject.FindProperty(nameof(_childBinders));
            _subViews = serializedObject.FindProperty(nameof(_subViews));
            _parentView = serializedObject.FindProperty(nameof(_parentView));
            _showEditorLogs = serializedObject.FindProperty(nameof(_showEditorLogs));
        }

        public override void OnInspectorGUI()
        {
            DrawScriptTitle();
            
            // try to find parent view to define is it parent view or not
            AutoDefineParentView();
            DefineParentViews();

            var parentView = _parentView.objectReferenceValue as View;
            var searchProvider = CreateInstance<StringListSearchProvider>();
            var isParentViewExist = _parentViews.Count > 0;
            
            if (isParentViewExist && !string.IsNullOrEmpty(parentView?.ViewModelTypeFullName))
            {
                DrawEditorForSubView(searchProvider, parentView);
            }
            else
            {
                ViewModelsEditorUtility.DefineAllViewModels(_viewModelNames);
                DrawEditorForParentView(searchProvider);

                DrawDebug();

                DrawOpenViewModelButton(_view.ViewModelTypeFullName);
            }

            if (!_view.IsValidSetup())
            {
                DrawFixButton();
            }
        }

        private void DrawEditorForParentView()
        {
            
        }

        private void DrawEditorForSubView(StringListSearchProvider searchProvider, View parentView)
        {
            // parent MUST exist
            var isParentViewExist = parentView != null;
            if (!isParentViewExist)
            {
                throw new Exception("Parent view not found");
            }
            
            // If parent view model is empty, let's show a message about it
            var parentViewModelTypeFullName = parentView.ViewModelTypeFullName;
            if (string.IsNullOrEmpty(parentViewModelTypeFullName))
            {
                EditorGUILayout.HelpBox($"Parent View Model is not defined. Please, define View Model in the parent View: ({parentView.name})", MessageType.Warning);
                return; // Parent View Model isn't selected.
            }
            
            // If parent view model is NOT EMPTY, but couldn't be found, lets reset it
            var parentViewModelType = GetViewModelType(parentViewModelTypeFullName);
            if (parentViewModelType == null)
            {
                Debug.LogWarning("Parent view model is selected, but cannot be found. Reset parent view model to null.", parentView.gameObject);
                parentView.ResetViewModelTypeFullName();
                return;
            }
            
            var parentViewModelPropertyNames = GetAllViewModelPropertyNames(parentView.ViewModelTypeFullName);
            var isPropertySelected = !string.IsNullOrEmpty(_viewModelPropertyName.stringValue);
            if (isPropertySelected && !parentViewModelPropertyNames.Contains(_viewModelPropertyName.stringValue))
            {
                Debug.LogWarning("Parent view model was hanged, connected property name has been reset", parentView.gameObject);
                _viewModelPropertyName.stringValue = null;
                serializedObject.ApplyModifiedProperties();
            }
            
            DrawSearchPanelForViewModelProperties(searchProvider, parentView.ViewModelTypeFullName, parentViewModelPropertyNames);

            DrawDebug();

            var childViewModelType = GetChildViewModelType(parentView.ViewModelTypeFullName, _view.ViewModelPropertyName);
                
            DrawSubViewModelDebugButtons(parentView.gameObject, childViewModelType?.FullName);
        }
        
        private void DrawSearchPanelForViewModelProperties(StringListSearchProvider provider, string parentViewModelTypeFullName, ICollection<string> propertyNames)
        {
            var viewModelPropertiesWithSubViewModelNames = propertyNames.ToArray();

            provider.Init(viewModelPropertiesWithSubViewModelNames, viewModelPropertyWithSubViewModelName =>
            {
                _viewModelPropertyName.stringValue = viewModelPropertyWithSubViewModelName == MVVMConstants.NONE ? null : viewModelPropertyWithSubViewModelName;

                if (viewModelPropertyWithSubViewModelName != MVVMConstants.NONE)
                {
                    _viewModelTypeFullName.stringValue =
                        GetChildViewModelType(parentViewModelTypeFullName, viewModelPropertyWithSubViewModelName)
                            ?.GetGenericArguments().First().FullName;
                }

                надо еще инициировать проверку всех дочерних вьюх, т.к при смене вьюмодели или ссылки на свойство, дочерние вьюхи могут тоже поломаться
                
                serializedObject.ApplyModifiedProperties();
            });
                
            EditorGUILayout.BeginHorizontal();

            var childPropertyType = GetChildViewModelType(parentViewModelTypeFullName, _viewModelPropertyName.stringValue);
            var propertyTypeName = childPropertyType != null ? $" ({childPropertyType.GetGenericArguments()[0].Name})" : string.Empty;
            
            EditorGUILayout.LabelField($"Property Name{propertyTypeName}:");

            var displayName = string.IsNullOrEmpty(_viewModelPropertyName.stringValue)
                ? MVVMConstants.NONE
                : _viewModelPropertyName.stringValue;
            
            if (GUILayout.Button(displayName, EditorStyles.popup))
            {
                SearchWindow.Open(new SearchWindowContext(GUIUtility.GUIToScreenPoint(Event.current.mousePosition)), provider);
            }
            
            EditorGUILayout.EndHorizontal();
        }

        private void AutoDefineParentView()
        {
            if (_parentView.objectReferenceValue != null)
            {
                _isParentView.boolValue = false;
                serializedObject.ApplyModifiedProperties();
                
                var parentViews = _view.GetComponentsInParent<View>();
                if (parentViews.Any(pv => ReferenceEquals(pv, _parentView.objectReferenceValue)))
                {
                    return;
                }
                
                EditorGUILayout.HelpBox("Cannot see parent View, but the reference still exists. Maybe you've changed the hierarchy of GameObjects and parent View has been lost.", MessageType.Error);

                return;
            }

            var parent = _view.transform.parent;
            if (parent == null)
            {
                // root View
                _isParentView.boolValue = true;
                serializedObject.ApplyModifiedProperties();
                return;
            }
            
            var parentView = _view.GetComponentsInParent<View>().FirstOrDefault(c => !ReferenceEquals(c, _view));
            if (parentView != null)
            {
                _isParentView.boolValue = false;
                _parentView.objectReferenceValue = parentView;
                serializedObject.ApplyModifiedProperties();
            }
        }

        private void DefineParentViews()
        {
            _parentViews.Clear();
            
            var parent = _view.transform.parent;
            if (parent != null)
            {
                var foundParentViews = parent.GetComponentsInParent<View>();
                _parentViews.AddRange(foundParentViews);
            }
        }

        private Type GetViewModelType(string viewModelTypeFullName)
        {
            var type = ViewModelsDB.AllViewModelTypes.FirstOrDefault(t => t.FullName == viewModelTypeFullName);
            return type;
        }

       

        private void DrawEditorForParentView(StringListSearchProvider provider)
        {
            var viewModelTypeFullNames = _viewModelNames.Values.ToArray();

            provider.Init(viewModelTypeFullNames, viewModelTypeFullName =>
            {
                _viewModelTypeFullName.stringValue = viewModelTypeFullName == MVVMConstants.NONE ? null : viewModelTypeFullName;
                serializedObject.ApplyModifiedProperties();
                
                надо еще инициировать проверку всех дочерних вьюх, т.к при смене вьюмодели или ссылки на свойство, дочерние вьюхи могут тоже поломаться

            });
                
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(MVVMConstants.VIEW_MODEL);

            var displayName = string.IsNullOrEmpty(_viewModelTypeFullName.stringValue)
                ? MVVMConstants.NONE
                : ViewModelsEditorUtility.ToShortName(_viewModelTypeFullName.stringValue);
            
            if (GUILayout.Button(displayName, EditorStyles.popup))
            {
                var mousePos = Event.current.mousePosition;
                mousePos.Set(mousePos.x - 200, mousePos.y);
                SearchWindow.Open(new SearchWindowContext(GUIUtility.GUIToScreenPoint(mousePos), 400), provider);
            }
            
            EditorGUILayout.EndHorizontal();
        }

        private ICollection<string> GetAllViewModelPropertyNames(string parentViewModelTypeFullName)
        {
            _viewModelPropertyNames.Clear();

            var parentViewModelType = GetViewModelType(parentViewModelTypeFullName);

            if (parentViewModelType == null)
            {
                EditorGUILayout.HelpBox(
                    $"ViewModel for Parent View didn't selected, the list of properties cannot be defined. Please, check the View Model setup for parent View ({(_parentView.objectReferenceValue as View)?.name})",
                    MessageType.Warning);

                return _viewModelPropertyNames; // parent view model is not selected
            }

            var allViewModelProperties = parentViewModelType.GetProperties();
            var allValidProperties =
                allViewModelProperties.Where(p =>
                {
                    var genericArgument = p.PropertyType.GetGenericArguments().First();
                    return typeof(IViewModel).IsAssignableFrom(genericArgument);
                });

            foreach (var validProperty in allValidProperties)
            {
                _viewModelPropertyNames.Add(validProperty.Name);
            }

            return _viewModelPropertyNames;
        }

        private void DrawScriptTitle()
        {
            GUI.enabled = false;
            EditorGUILayout.ObjectField(MVVMConstants.SCRIPT, MonoScript.FromMonoBehaviour((View)target), typeof(View), false);
            GUI.enabled = true;
        }
        
        private void DrawDebug()
        {
            _showEditorLogs.boolValue = EditorGUILayout.Foldout(_showEditorLogs.boolValue, "Logs");
            serializedObject.ApplyModifiedProperties();

            if (_showEditorLogs.boolValue)
            {
                EditorGUI.indentLevel++;
                GUI.enabled = false;
                EditorGUILayout.PropertyField(_isParentView);

                if (_parentView.objectReferenceValue != null)
                {
                    EditorGUILayout.PropertyField(_parentView);
                }
                
                EditorGUILayout.PropertyField(_subViews);
                EditorGUILayout.PropertyField(_childBinders);
                GUI.enabled = true;
                EditorGUI.indentLevel--;
            }
        }

        private void DrawPingParentViewButton(GameObject parentViewGo)
        {
            if (parentViewGo != null && GUILayout.Button(MVVMConstants.HIGHLIGHT_PARENT_VIEW))
            {
                EditorGUIUtility.PingObject(parentViewGo);
            }
        }

        private void DrawOpenViewModelButton(string viewModelTypeFullName)
        {
            if (!string.IsNullOrEmpty(viewModelTypeFullName))
            {
                var viewModelType = GetViewModelType(viewModelTypeFullName);

                if (viewModelType == null)
                {
                    return;
                }

                if (GUILayout.Button($"Open {viewModelType.Name}"))
                {
                    OpenScript(viewModelType.Name);
                }
            }
        }

        private void DrawSubViewModelDebugButtons(GameObject parentViewGo, string viewModelTypeFullName)
        {
            EditorGUILayout.BeginHorizontal();

            DrawPingParentViewButton(parentViewGo);
            DrawOpenViewModelButton(viewModelTypeFullName);
            
            EditorGUILayout.EndHorizontal();
        }
        
        private Type GetChildViewModelType(string parentViewModelTypeFullName, string propertyName)
        {
            if (!string.IsNullOrEmpty(propertyName))
            {
                var parentViewModelType = GetViewModelType(parentViewModelTypeFullName);
                var property = parentViewModelType.GetProperty(propertyName);
  
                return property.PropertyType;
            }
            
            return null;
        }
        
        private void OpenScript(string typeName)
        {
            var guids = AssetDatabase.FindAssets($"t: script {typeName}");

            if (guids.Length > 0)
            {
                var scriptPath = AssetDatabase.GUIDToAssetPath(guids[0]);
                
                EditorUtility.OpenWithDefaultApp(scriptPath);
            }
            else
            {
                Debug.LogError($"No script found for type: {typeName}");
            }
        }

        private void DrawFixButton()
        {
            EditorGUILayout.HelpBox("Some binders or sub views are missing. Please, fix it", MessageType.Warning);
            
            if (GUILayout.Button($"Fix"))
            {
                _view.Fix();
            }
        }
    }
}