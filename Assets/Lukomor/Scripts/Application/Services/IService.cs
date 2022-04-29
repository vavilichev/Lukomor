using System;
using System.Threading.Tasks;

namespace Lukomor.Application.Services
{
	public interface IService : IDisposable
	{
		bool IsReady { get; }

		Task Initialize();
	}
}