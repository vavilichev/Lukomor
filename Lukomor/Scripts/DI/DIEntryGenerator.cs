using System;

namespace Lukomor.DI
{
    public sealed class DIEntryGenerator<T>: DiEntry<T>
    {
        public DIEntryGenerator(DIContainer container, Func<DIContainer, T> factory) : base(container, factory) { }
        
        public override T Resolve()
        {
            return Factory(Container);
        }
    }
}