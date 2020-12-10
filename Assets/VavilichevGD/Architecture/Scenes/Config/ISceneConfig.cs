using System;
using System.Collections.Generic;

namespace VavilichevGD.Architecture {
	public interface ISceneConfig {
		
		string sceneName { get; }
		
		
		Dictionary<Type, IRepository> CreateAllRepositories();
		Dictionary<Type, IInteractor> CreateAllInteractors();
	}
}