using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Lukomor.Common.Utils.Async {
	public class UnityAwaiters {
		private static readonly WaitForEndOfFrame endOfFrameWaiter = new WaitForEndOfFrame();

        public static async Task WaitForSeconds(float seconds, CancellationToken cancellationToken = default)
        {
#if UNITY_EDITOR
            if (!UnityEngine.Application.isPlaying)
            {
                // When the editor is not playing
                // scaled time is the same as realtime
                await WaitForSecondsRealtime(seconds, cancellationToken);
                return;
            }
#endif
            float endTime = Time.time + seconds;
            while (Time.time < endTime)
            {
                cancellationToken.ThrowIfCancellationRequested();
                await Task.Yield();
            }
        }
        
        public static async Task WaitForSecondsRealtime(float seconds, CancellationToken cancellationToken = default)
        {
#if UNITY_EDITOR
            if (!UnityEngine.Application.isPlaying)
            {
                float editorEndTime = (float)EditorApplication.timeSinceStartup + seconds;
                while ((float)EditorApplication.timeSinceStartup < editorEndTime)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    await Task.Yield();
                }

                return;
            }
#endif

            float endTime = Time.realtimeSinceStartup + seconds;
            while (Time.realtimeSinceStartup < endTime)
            {
                cancellationToken.ThrowIfCancellationRequested();
                await Task.Yield();
            }
        }

        public static async Task WaitUntil(Func<bool> predicate, CancellationToken cancellationToken = default)
        {
            while (!predicate.Invoke())
            {
                cancellationToken.ThrowIfCancellationRequested();
                await Task.Yield();
            }
        }

        public static async Task WaitWhile(Func<bool> predicate, CancellationToken cancellationToken = default)
        {
            while (predicate.Invoke())
            {
                cancellationToken.ThrowIfCancellationRequested();
                await Task.Yield();
            }
        }

        public static Task WaitForTime(TimeSpan timeSpan, CancellationToken cancellationToken = default)
        {
            return WaitForSeconds((float)timeSpan.TotalSeconds, cancellationToken);
        }

        public static Task WaitForRealtime(TimeSpan timeSpan, CancellationToken cancellationToken = default)
        {
            return WaitForSecondsRealtime((float)timeSpan.TotalSeconds, cancellationToken);
        }

        public static async Task WaitForFrames(int frameCount, CancellationToken cancellationToken = default)
        {
            int current = 0;
            while (current < frameCount)
            {
                await WaitNextFrame(cancellationToken);
                current++;
            }
        }

        public static YieldAwaitable WaitNextFrame()
        {
            return Task.Yield();
        }

        public static async Task WaitNextFrame(CancellationToken cancellationToken)
        {
            await Task.Yield();
            cancellationToken.ThrowIfCancellationRequested();
        }
	}
}