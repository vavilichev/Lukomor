namespace VavilichevGD.Architecture.Example {
	public class GameManagerExample : GameManager{
		protected override void OnGameLaunched() {
			GameExample.Run();
		}
	}
}