using System.Collections.Generic;
using Lukomor.Common;
using UnityEngine;

namespace Lukomor.Presentation.Views
{
    public abstract class ViewModel : MonoBehaviour
    {
        public IView View { get; private set; }
        public bool IsActive => View.IsActive;
        
        private readonly Dictionary<string, object> _payloadsMap = new Dictionary<string, object>();
        
        private void Awake()
        {
            View = GetComponent<IView>();
            
            AwakeInternal();
        }

        protected virtual void AwakeInternal() { }

        public void AddPayloads(params Payload[] payloads)
        {
            foreach (var payload in payloads)
            {
                _payloadsMap[payload.Key] = payload.Value;
            }
            
            PayloadsAdded();
        }

        public void Refresh()
        {
            RefreshInternal();
            
            View.Refresh();
        }

        public void Subscribe()
        {
            SubscribeInternal();
            
            View.Subscribe();
        }

        public void Unsubscribe()
        {
            UnsubscribeInternal();
            
            View.Unsubscribe();
        }

        protected virtual void PayloadsAdded() { }
        protected virtual void RefreshInternal() { }
        protected virtual void SubscribeInternal() { }
        protected virtual void UnsubscribeInternal() { }

        protected bool TryGetPayload<T>(string key, out T payload)
        {
            payload = default;
            
            var payloadExists = _payloadsMap.TryGetValue(key, out object payloadObject);
            
            if (payloadExists)
            {
                payload = (T) payloadObject;
            }

            return payloadExists;
        }

        protected void RemovePayload(string key)
        {
            if (_payloadsMap.ContainsKey(key))
            {
                _payloadsMap.Remove(key);
            }
        }
    }
}