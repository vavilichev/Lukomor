using System;

namespace Lukomor.DI
{
    public sealed class DIEntrySingleton<T> : DIEntry<T>
    {
        private T _instance;
        
        public DIEntrySingleton(DIContainer diContainer, Func<DIContainer, T> factory) : base(diContainer, factory) { }
        
        public override T Resolve()
        {
            if (_instance == null)
            {
                _instance = Factory(DiContainer);
            }

            return _instance;
        }
    }
}