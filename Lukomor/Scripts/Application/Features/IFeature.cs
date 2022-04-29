using System;
using System.Threading.Tasks;

namespace Lukomor.Application.Features
{
	public interface IFeature : IDisposable
	{
		bool IsReady { get; }

		Task Initialize();
	}
}