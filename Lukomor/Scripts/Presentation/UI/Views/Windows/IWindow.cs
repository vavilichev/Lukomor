using System;
using System.Threading.Tasks;

namespace Lukomor.Presentation.Views.Windows {
	public interface IWindow<out TWindowViewModel> : IWindow, IView<TWindowViewModel>
		where TWindowViewModel : WindowViewModel { }

	public interface IWindow : IView
	{
		event Action<WindowViewModel> Hidden;
		event Action<WindowViewModel> Destroyed;
		event Action<bool> BlockInteractingRequested;
		
		bool IsShown { get; }
		
		Task<IWindow> Show();
		Task<IWindow> Hide();
		IWindow HideInstantly();
	}
}