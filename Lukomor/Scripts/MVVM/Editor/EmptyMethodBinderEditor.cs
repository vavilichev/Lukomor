using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Lukomor.MVVM.Binders;
using UnityEditor;

namespace Lukomor.MVVM.Editor
{
    [CustomEditor(typeof(EmptyMethodBinder))]
    public class EmptyMethodBinderEditor : MethodBinderEditor
    {
        protected override IEnumerable<MethodInfo> GetMethodsInfo()
        {
            var viewModelType = Type.GetType(ViewModelTypeFullName.stringValue);
            var allMethods = viewModelType.GetMethods()
                .Where(m => m.GetParameters().Length == 0 && m.ReturnType == typeof(void));

            return allMethods;
        }
    }
}