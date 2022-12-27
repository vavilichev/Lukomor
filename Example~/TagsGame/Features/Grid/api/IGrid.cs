namespace Lukomor.TagsGame.Grid
{
    public interface IGrid
    {
        ICell[] Cells { get; }
        int Size { get; }

        void Randomize();
    }
}