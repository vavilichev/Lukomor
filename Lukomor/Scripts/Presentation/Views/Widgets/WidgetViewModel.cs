using UnityEngine;

namespace Lukomor.Presentation.Views.Widgets
{
    public class WidgetViewModel : ViewModel
    {
        [Tooltip("If enabled you have to call Refresh() method manually for refreshing widget. If not - it calls automatically from the OnEnable() method")]
        [SerializeField] private bool _manualRefreshing;
        [Tooltip("If enabled you have to call Subscribe() and Unsubscribe methods manually. If not - it calls automatically from the OnEnable() and OnDisable() methods")]
        [SerializeField] private bool _manualSubscription;
        
        public IWidget Widget {
            get
            {
                if (_widget == null)
                {
                    _widget = (IWidget) View;
                }

                return _widget;
            }
        }

        private IWidget _widget;

        private void OnEnable()
        {
            if (!_manualRefreshing)
            {
                Refresh();
            }

            if (!_manualSubscription)
            {
                Subscribe();
            }
            
            OnEnableInternal();
        }

        private void OnDisable()
        {
            if (!_manualSubscription)
            {
                Unsubscribe();
            }

            OnDisableInternal();
        }
        
        protected virtual void OnEnableInternal() { }
        protected virtual void OnDisableInternal() { }
    }
}