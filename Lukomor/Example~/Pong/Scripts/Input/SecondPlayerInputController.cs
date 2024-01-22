using UnityEngine;

namespace Lukomor.Example.Pong
{
    public class SecondPlayerInputController : InputController
    {
        private void Update()
        {
            var y = 0;

            if (Input.GetKey(KeyCode.UpArrow))
            {
                y += 1;
            }

            if (Input.GetKey(KeyCode.DownArrow))
            {
                y -= 1;
            }
            
            Block.Move(y);
        }
    }
}