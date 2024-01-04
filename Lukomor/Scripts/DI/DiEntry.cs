using System;

namespace Lukomor.DI
{
    public abstract class DIEntry
    {
        protected DIContainer Container { get; }
        
        public DIEntry(DIContainer container)
        {
            Container = container;
        }
        
        public T Resolve<T>()
        {
            return ((DiEntry<T>)this).Resolve();
        }
    }
    
    public abstract class DiEntry<T> : DIEntry
    {
        protected Func<DIContainer, T> Factory { get; }
        
        public DiEntry(DIContainer container, Func<DIContainer, T> factory) : base(container)
        {
            Factory = factory;
        }

        public abstract T Resolve();
    }
}