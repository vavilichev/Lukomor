using UnityEngine;

namespace Lukomor.Example.Pong
{
    public abstract class InputController : MonoBehaviour
    {
        protected BlockView Block { get; private set; }
        
        public void Bind(BlockView block)
        {
            Block = block;
        }
    }
}