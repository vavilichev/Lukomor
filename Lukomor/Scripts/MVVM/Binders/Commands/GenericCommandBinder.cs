using UnityEngine;

namespace Lukomor.MVVM.Binders
{
    public abstract class GenericCommandBinder<T> : CommandBinder<T>
    {
        [SerializeField] private T _localParam;

        public void ExecuteCommandWithLocalParam()
        {
            ExecuteCommand(_localParam);
        }
    }
}