using System;

namespace Lukomor.DI
{
    public abstract class DIEntry
    {
        protected DIContainer DiContainer { get; }
        
        public DIEntry(DIContainer diContainer)
        {
            DiContainer = diContainer;
        }
        
        public T Resolve<T>()
        {
            return ((DIEntry<T>)this).Resolve();
        }
    }
    
    public abstract class DIEntry<T> : DIEntry
    {
        protected Func<DIContainer, T> Factory { get; }
        
        public DIEntry(DIContainer diContainer, Func<DIContainer, T> factory) : base(diContainer)
        {
            Factory = factory;
        }

        public abstract T Resolve();
    }
}