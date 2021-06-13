using System.Collections.Generic;

namespace VavilichevGD.Architecture.Extentions {
    public static class ArchitectureExtensions {
        public static T GetRepository<T>(this object o) where T : IRepository {
            return Game.GetRepository<T>();
        }
        
        public static T GetInteractor<T>(this object o) where T : IInteractor {
            return Game.GetInteractor<T>();
        }
        
        public static IEnumerable<T> GetInteractors<T>(this object o) where T : IInteractor {
            return Game.GetInteractors<T>();
        }
        
        public static IEnumerable<T> GetRepositories<T>(this object o) where T : IRepository {
            return Game.GetRepositories<T>();
        }
    }
}