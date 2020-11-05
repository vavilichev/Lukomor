VavilichevGD.Architecture

DETAILS:
This folder responsible for initializing all game data. The Game script is abstract form of main script that controls the process. 
RepositoriesBase controls all repositories and InteractorsBase - interactors. 
You have to store the game state in different repositories. You can load it locally, remotely, and ofcourse save it locally and remotely too. 
Interactors - it is about business logic of the game. This scripts stores temp data and controls it.

YOUR PROJECT:
If you want to make your own architecture template, please inherit from Game.cs, Interactor.cs, InteractorsBase.cs, Repository.cs, RepositoriesBase.cs scripts.

EXAMPLE:
If you want to look example, you may load ExampleArchitecture scene. Do not forget uncomment line with [RuntimeInitializeOnLoadMethod] in the GameExample.cs. 
Just run the scene and look at console. You may explore scripts in Example folder for better understanding. 
Comment line of code again otherwise it will be running in your project.