namespace Lukomor.Presentation.Views
{
    public static class ViewExtensions
    {
        public static T As<T>(this IView view) where T : IView
        {
            return (T) view;
        }
    }
}