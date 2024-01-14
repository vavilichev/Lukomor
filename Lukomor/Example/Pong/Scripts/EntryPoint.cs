using System;
using UnityEngine;

namespace Lukomor.Example.Pong
{
    public class EntryPoint : MonoBehaviour
    {
        [SerializeField] private Block _leftBlock;
        [SerializeField] private Block _rightBlock;
        [SerializeField] private Ball _ball;

        private void Start()
        {
            SetupPlayer<FirstPlayerInputController>(_leftBlock);
            SetupAI(_rightBlock, _ball);
        }
        
        private void SetupPlayer<T>(Block block) where T : InputController
        {
            var inputController = block.GetComponent<InputController>();

            if (!inputController)
            {
                inputController = block.gameObject.AddComponent<T>();
            }
            
            inputController.Bind(block);
        }

        private void SetupAI(Block block, Ball ball)
        {
            var inputController = block.GetComponent<AIInputController>();

            if (!inputController)
            {
                inputController = block.gameObject.AddComponent<AIInputController>();
            }
            
            inputController.Bind(block, ball);
        }
    }
}