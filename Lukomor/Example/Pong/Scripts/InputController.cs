using UnityEngine;

namespace Lukomor.Example.Pong
{
    public abstract class InputController : MonoBehaviour
    {
        protected Block Block { get; private set; }
        
        public void Bind(Block block)
        {
            Block = block;
        }
    }
}