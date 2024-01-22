using UnityEngine;

namespace Lukomor.Example.Pong
{
    public class FirstPlayerInputController : InputController
    {
        private void Update()
        {
            var y = 0;

            if (Input.GetKey(KeyCode.W))
            {
                y += 1;
            }

            if (Input.GetKey(KeyCode.S))
            {
                y -= 1;
            }
            
            Block.Move(y);
        }
    }
}