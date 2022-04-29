using System;
using System.Threading.Tasks;
using Lukomor.Presentation.Common;
using UnityEngine;

namespace Lukomor.Presentation.Views.Windows {
	public interface IWindow : IView {

		event Action<IWindow> Hidden;
		event Action<IWindow> Destroyed; 
		event Action<bool> BlockInteractingRequested;
		
		GameObject GameObject { get; }
		bool IsPreCached { get; }
		bool OpenedByDefault { get; }
		bool CloseWhenUnfocused { get; }
		bool IsActive { get; }
		UILayer TargetLayer { get; }
		UserInterface UI { get; set; }

		void Install();
		void Refresh();
		void Subscribe();
		void Unsubscribe();
		
		Task<IWindow> Show();
		Task<IWindow> Hide();
		IWindow HideInstantly();
	}
}