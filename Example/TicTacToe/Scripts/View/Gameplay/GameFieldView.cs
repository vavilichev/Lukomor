using Lukomore.Example.TicTacToe.Gameplay;
using UnityEngine;

namespace Lukomore.Example.TicTacToe.View.Gameplay
{
    public class GameFieldView : MonoBehaviour
    {
        private Field _field;
        
        public void Init(Field field)
        {
            _field = field;
            
            Fill();
        }

        private void Fill()
        {
            
        }
    }
}