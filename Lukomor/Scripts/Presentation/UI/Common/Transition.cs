using System.Threading.Tasks;
using UnityEngine;

namespace Lukomor.Presentation.Common {
	public abstract class Transition : MonoBehaviour {
		public abstract Task Play();
	}
}