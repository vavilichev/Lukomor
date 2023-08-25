namespace Lukomore.Example.TicTacToe.Gameplay
{
    public class GameplayFeature
    {
        public Field Field { get; }
        
        public GameplayFeature()
        {
            Field = new Field(3, 3);
            
            Field.CellStateChanged += OnFieldCellStateChanged;
        }

        private void OnFieldCellStateChanged(ICell cell)
        {
            CheckWin();
        }

        private void CheckWin()
        {
            
        }
    }
}