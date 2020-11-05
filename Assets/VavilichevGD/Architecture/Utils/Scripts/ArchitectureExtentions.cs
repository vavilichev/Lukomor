using UnityEngine;

namespace VavilichevGD.Architecture.Extentions {
    public static class ArchitectureExtentions {

        public static T GetInteractor<T>(this MonoBehaviour mono) where T : Interactor {
            return Game.GetInteractor<T>();
        }

        public static T GetRepository<T>(this MonoBehaviour mono) where T : Repository {
            return Game.GetRepository<T>();
        }
    }
}