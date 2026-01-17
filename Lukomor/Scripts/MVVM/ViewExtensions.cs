using System.Collections.Generic;
using System.Linq;

namespace Lukomor.MVVM
{
    public static class ViewExtensions
    {
        public static View FirstOrDefaultParentView(this View view)
        {
            var result = view.GetComponentsInParent<View>().FirstOrDefault(v => !ReferenceEquals(v, view));
            return result;
        }

        public static IEnumerable<View> AllParentViews(this View view)
        {
            var result = view.GetComponentsInParent<View>().Where(v => !ReferenceEquals(v, view));
            return result;
        }

        public static IEnumerable<View> AllSubViews(this View view)
        {
            var result = view.GetComponentsInChildren<View>().Where(v => !ReferenceEquals(v, view));
            return result;
        }

        public static int GetId(this View view)
        {
            return view.gameObject.GetInstanceID();
        }
    }
}