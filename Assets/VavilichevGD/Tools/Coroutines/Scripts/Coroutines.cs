using System.Collections;
using  UnityEngine;

namespace VavilichevGD.Tools
{
    public class Coroutines : MonoBehaviour {
        private static Coroutines instance => GetInstance();
        private static Coroutines m_instance;
        private static bool isInitialized => m_instance != null;
        
        private const string NAME = "[COROUTINE MANAGER]";

        private static Coroutines GetInstance() {
            if (!isInitialized)
                m_instance = CreateSingleton();
            return m_instance;
        }

        private static Coroutines CreateSingleton() {
            Coroutines createdManager = new GameObject(NAME).AddComponent<Coroutines>();
            createdManager.hideFlags = HideFlags.HideAndDontSave;
            DontDestroyOnLoad(createdManager.gameObject);
            return createdManager;
        }

        public static Coroutine StartRoutine(IEnumerator enumerator) {
            return instance.StartCoroutine(enumerator);
        }

        public static void StopRoutine(Coroutine routine) {
            if (routine != null)
                instance.StopCoroutine(routine);
        }

        public static void StopRoutine(string routineName) {
            instance.StopCoroutine(routineName);
        }
    }
}