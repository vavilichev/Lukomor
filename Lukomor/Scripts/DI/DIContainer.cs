using System;
using System.Collections.Generic;

namespace Lukomor.DI
{
    public sealed class DIContainer
    {
        private Dictionary<(string, Type), DIEntry> _factoriesMap = new();
        private HashSet<(string, Type)> _cachedKeysForResolving = new();

        private readonly DIContainer _parentContainer;
        
        public DIContainer(DIContainer parentContainer = null)
        {
            _parentContainer = parentContainer;
        }
        
        public void RegisterSingleton<T>(Func<DIContainer, T> factory)
        {
            RegisterSingleton("", factory);
        }

        public void RegisterSingleton<T>(string tag, Func<DIContainer, T> factory)
        {
            var key = (tag, typeof(T));
            
            RegisterSingleton(key, factory);
        }

        public void Register<T>(Func<DIContainer, T> factory)
        {
            Register("", factory);
        }

        public void Register<T>(string tag, Func<DIContainer, T> factory)
        {
            var key = (tag, typeof(T));
            
            Register(key, factory);
        }

        public T Resolve<T>(string key = "")
        {
            // TODO: Остановился на том, что надо зарезолвить, и учесть цикличные резолвы.
        }
        
        private void RegisterSingleton<T>((string, Type) key, Func<DIContainer, T> factory)
        {
            if (_factoriesMap.ContainsKey(key))
            {
                throw new Exception("Already has factory entry for type: " + key.Item2.Name);
            }
            
            var diEntry = new DIEntrySingleton<T>(this, factory);

            _factoriesMap[key] = diEntry;
        }

        private void Register<T>((string, Type) key, Func<DIContainer, T> factory)
        {
            if (_factoriesMap.ContainsKey(key))
            {
                throw new Exception("Already has factory entry for type: " + key.Item2.Name);
            }
            
            var diEntry = new DIEntryGenerator<T>(this, factory);

            _factoriesMap[key] = diEntry;
        }
    }
}