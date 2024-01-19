using UnityEngine;

namespace Lukomor.Example.Pong
{
    public abstract class PongInputController : MonoBehaviour
    {
        protected PongBlockView Block { get; private set; }
        
        public void Bind(PongBlockView block)
        {
            Block = block;
        }
    }
}