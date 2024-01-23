using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Lukomor.MVVM.Editor
{
    public class StringListSearchProvider: ScriptableObject, ISearchWindowProvider
    {
        private string[] _options;
        private Action<string> _callback;
        
        public void Init(string[] options, Action<string> callback)
        {
            _options = options;
            _callback = callback;
        }
        
        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            var searchList = new List<SearchTreeEntry>();

            searchList.Add(new SearchTreeGroupEntry(new GUIContent(MVVMConstants.SEARCH), 0));

            foreach (var option in _options)
            {
                var entry = new SearchTreeEntry(new GUIContent(option));
                entry.level = 1;
                entry.userData = option;
                searchList.Add(entry);
            }
            
            return searchList;
        }

        public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
        {
            _callback?.Invoke((string) SearchTreeEntry.userData);
            return true;
        }
    }
}