using System;
using System.Collections.Generic;
using VavilichevGD.Architecture.Example;

namespace VavilichevGD.Architecture.Scenes {
    public sealed class SceneConfigExample : SceneConfigBase {

        #region CONSTANTS

        public const string SCENE_NAME = "GameArchitectureExample";

        #endregion
        
        public override string sceneName { get; }

        public SceneConfigExample() {
            this.sceneName = SCENE_NAME;
        }

        public override Dictionary<Type, IRepository> CreateAllRepositories() {
            var createdReposisories = new Dictionary<Type, IRepository>();

            this.CreateRepository<DummyRepository>(createdReposisories);

            return createdReposisories;
        }

        public override Dictionary<Type, IInteractor> CreateAllInteractors() {
            var createdInteractors = new Dictionary<Type, IInteractor>();
            
            this.CreateInteractor<DummyInteractor>(createdInteractors);
            
            return createdInteractors;
        }

    }
}