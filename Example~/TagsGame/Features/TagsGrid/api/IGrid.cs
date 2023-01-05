namespace Lukomor.TagsGame.TagsGrid
{
    public interface IGrid
    {
        ICell[] Cells { get; }
        int Size { get; }

        void Randomize();
    }
}