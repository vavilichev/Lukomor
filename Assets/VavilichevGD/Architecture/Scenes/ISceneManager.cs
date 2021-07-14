using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace VavilichevGD.Architecture {
    public delegate void SceneManagerHandler(SceneConfig config);
    
    public interface ISceneManager {

        #region EVENTS

        event SceneManagerHandler OnSceneLoadStartedEvent;
        event SceneManagerHandler OnSceneLoadCompletedEvent;

        #endregion

        IScene sceneActual { get; }
        Dictionary<string, SceneConfig> scenesConfigMap { get; }

        Coroutine LoadScene(string sceneName, UnityAction<SceneConfig> sceneLoadedCallback = null);
        Coroutine InitializeCurrentScene(UnityAction<SceneConfig> sceneLoadedCallback = null);
    }
}