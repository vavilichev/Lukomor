using System;
using System.Collections.Generic;

namespace Lukomor.DI
{
    public sealed class DIContainer
    {
        private readonly Dictionary<(string, Type), DIEntry> _factoriesMap = new();
        private readonly HashSet<(string, Type)> _cachedKeysForResolving = new();

        private readonly DIContainer _parentDiContainer;
        
        public DIContainer(DIContainer parentDiContainer = null)
        {
            _parentDiContainer = parentDiContainer;
        }
        
        public DIBuilder<T> RegisterSingleton<T>(Func<DIContainer, T> factory)
        {
            return RegisterSingleton("", factory);
        }

        public DIBuilder<T> RegisterSingleton<T>(string tag, Func<DIContainer, T> factory)
        {
            var key = (tag, typeof(T));
            
            return RegisterSingleton(key, factory);
        }

        public DIBuilder<T> Register<T>(Func<DIContainer, T> factory)
        {
            return Register("", factory);
        }

        public DIBuilder<T> Register<T>(string tag, Func<DIContainer, T> factory)
        {
            var key = (tag, typeof(T));
            
            return Register(key, factory);
        }

        public T Resolve<T>(string tag = "")
        {
            var type = typeof(T);
            var key = (tag, type);

            if (_cachedKeysForResolving.Contains(key))
            {
                throw new Exception($"Cyclic dependencies. Key: {key}");
            }

            _cachedKeysForResolving.Add(key);

            T result;

            if (!_factoriesMap.ContainsKey(key))
            {
                if (_parentDiContainer == null)
                {
                    throw new Exception($"There is no factory registered for key: {key}");
                }

                result = _parentDiContainer.Resolve<T>(tag);
            }
            else
            {
                result = _factoriesMap[key].Resolve<T>();
            }
            
            _cachedKeysForResolving.Remove(key);

            return result;
        }
        
        private DIBuilder<T> RegisterSingleton<T>((string, Type) key, Func<DIContainer, T> factory)
        {
            if (_factoriesMap.ContainsKey(key))
            {
                throw new Exception("Already has factory entry for key: " + key);
            }
            
            var diEntry = new DIEntrySingleton<T>(this, factory);

            _factoriesMap[key] = diEntry;

            return new DIBuilder<T>(diEntry);
        }

        private DIBuilder<T> Register<T>((string, Type) key, Func<DIContainer, T> factory)
        {
            if (_factoriesMap.ContainsKey(key))
            {
                throw new Exception("Already has factory entry for type: " + key.Item2.Name);
            }
            
            var diEntry = new DIEntryTransient<T>(this, factory);

            _factoriesMap[key] = diEntry;

            return new DIBuilder<T>(diEntry);
        }
    }
}