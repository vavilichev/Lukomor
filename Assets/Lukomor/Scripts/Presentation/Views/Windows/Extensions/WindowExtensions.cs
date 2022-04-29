using System.Collections.Generic;
using System.Linq;

namespace Lukomor.Presentation.Views.Windows.Extensions {
	public static class WindowExtensions {
		public static bool ContainsWindow(this IEnumerable<IWindow> windows, IWindow window) {
			return windows.Any(w => w.GetType() == window.GetType());
		}
	}
}