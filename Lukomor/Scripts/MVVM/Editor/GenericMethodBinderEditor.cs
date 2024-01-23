using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Lukomor.MVVM.Binders;
using UnityEditor;

namespace Lukomor.MVVM.Editor
{
    [CustomEditor(typeof(GenericMethodBinder), true)]
    public class GenericMethodBinderEditor : MethodBinderEditor
    {
        private GenericMethodBinder _genericMethodBinder;
        private Type _parameterType;

        protected override void OnEnable()
        {
            base.OnEnable();

            _genericMethodBinder = (GenericMethodBinder)target;
        }

        protected override IEnumerable<MethodInfo> GetMethodsInfo()
        {
            var viewModelType = GetViewModelType(ViewModelTypeFullName.stringValue);
            var requiredType = _genericMethodBinder.ParameterType;

            var allMethods = viewModelType.GetMethods()
                .Where(m =>
                {
                    var allParameters = m.GetParameters();

                    if (allParameters.Length != 1)
                    {
                        return false;
                    }

                    if (m.ReturnType != typeof(void))
                    {
                        return false;
                    }

                    var parameterType = allParameters[0].ParameterType;

                    return parameterType == requiredType;
                });

            return allMethods;
        }
    }
}