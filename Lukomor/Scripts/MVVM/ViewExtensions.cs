using System.Linq;

namespace Lukomor.MVVM
{
    public static class ViewExtensions
    {
        public static View FirstOrDefaultSourceView(this View view)
        {
            var result = view.GetComponentsInParent<View>().FirstOrDefault(v => !ReferenceEquals(v, view));
            return result;
        }
    }
}