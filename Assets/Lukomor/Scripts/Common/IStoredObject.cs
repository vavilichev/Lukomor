using System.Threading.Tasks;

namespace Lukomor.Common
{
	public interface IStoredObject
	{
		Task Save();
	}
}