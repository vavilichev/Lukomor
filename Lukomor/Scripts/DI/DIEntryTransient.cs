using System;

namespace Lukomor.DI
{
    public sealed class DIEntryTransient<T>: DIEntry<T>
    {
        public DIEntryTransient(DIContainer diContainer, Func<DIContainer, T> factory) : base(diContainer, factory) { }
        
        public override T Resolve()
        {
            return Factory(DiContainer);
        }
    }
}