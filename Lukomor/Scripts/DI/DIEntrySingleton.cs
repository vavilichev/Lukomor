using System;

namespace Lukomor.DI
{
    public sealed class DIEntrySingleton<T> : DiEntry<T>
    {
        private T _instance;
        
        public DIEntrySingleton(DIContainer container, Func<DIContainer, T> factory) : base(container, factory) { }
        
        public override T Resolve()
        {
            if (_instance == null)
            {
                _instance = Factory(Container);
            }

            return _instance;
        }
    }
}