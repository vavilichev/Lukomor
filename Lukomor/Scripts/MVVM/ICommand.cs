namespace Lukomor.MVVM
{
    public interface ICommand
    {
        void Execute();
    }

    public interface ICommand<in T>
    {
        void Execute(T parameter);
    }
}