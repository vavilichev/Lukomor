using JetBrains.Annotations;
using UnityEngine;

namespace VavilichevGD.Tools {
    public static class Logging {

        public static bool enabled => IsEnabled();

        private static bool IsEnabled() {
        #if DEBUG
            return true;
        #endif
            return false;
        }

        #region LOG

        public static void Log(string text, GameObject gameObject = null) {
#if DEBUG
            Debug.Log(text, gameObject);
#endif
        }

        public static void Log(GameObject gameObject, string format, [NotNull] params object[] args) {
#if DEBUG
            var text = string.Format(format, args);
            Debug.Log(text, gameObject);
#endif
        }
        
        public static void Log(string format, [NotNull] params object[] args) {
#if DEBUG
            var text = string.Format(format, args);
            Debug.Log(text);
#endif
        }

        #endregion


        #region LOG ERROR

        public static void LogError(string text, GameObject gameObject = null) {
#if DEBUG
	        Debug.LogError(text, gameObject);
#endif
        }
        
        public static void LogError(GameObject gameObject, string format, [NotNull] params object[] args) {
#if DEBUG
	        var text = string.Format(format, args);
	        Debug.LogError(text, gameObject);
#endif
        }
        
        public static void LogError(string format, [NotNull] params object[] args) {
#if DEBUG
	        var text = string.Format(format, args);
	        Debug.LogError(text);
#endif
        }

        #endregion
        
    }
}