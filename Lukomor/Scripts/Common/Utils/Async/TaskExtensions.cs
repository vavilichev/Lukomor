using System;
using System.Collections;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Lukomor.Common.Utils.Async {
	public static class TaskExtensions {
		/// <summary>
        /// Executes a task in a async void context. Uncaught exceptions are logged to the console
        /// </summary>
        [DebuggerHidden]
        public static async void RunAsync(this Task task)
        {
            /*
             * In a regular C# application, this would be dangerous. Uncaptured exceptions
             * inside an async void method cause the application to crash because they
             * can't be caught by the caller.
             *
             * However, this is Unity. Uncaught exceptions, are captured and Logged to the
             * console by Unity's SyncronizationContext.
             */
            await task;
        }

        /// <summary>
        /// Executes a task in a async void context. Uncaught exceptions are logged to the console
        /// </summary>
        [DebuggerHidden]
        public static async void RunAsync(this Task task, Action continuation)
        {
            /*
             * In a regular C# application, this would be dangerous. Uncaptured exceptions
             * inside an async void method cause the application to crash because they
             * can't be caught by the caller.
             *
             * However, this is Unity. Uncaught exceptions, are captured and Logged to the
             * console by Unity's SyncronizationContext.
             */
            await task;
            continuation?.Invoke();
        }

       public static IEnumerator AsIEnumerator(this Task task)
        {
            while (!task.IsCompleted)
            {
                yield return null;
            }

            if (task.IsFaulted)
            {
                throw task.Exception;
            }
        }
	}
}