using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Lukomor.MVVM.Editor
{
    public class StringListSearchProvider: ScriptableObject, ISearchWindowProvider
    {
        private readonly List<string> _options = new();
        private Action<string> _callback;
        
        public void Init(IEnumerable<string> options, Action<string> callback)
        {
            _options.Clear();
            _options.AddRange(options);
            _options.Sort();
            _options.Insert(0, MVVMConstants.NONE);

            _callback = callback;
        }
        
        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            var searchList = new List<SearchTreeEntry> { new SearchTreeGroupEntry(new GUIContent(MVVMConstants.SEARCH)) };

            foreach (var option in _options)
            {
                if (option == null)
                {
                    AddEntry(MVVMConstants.NONE, searchList);
                    continue;
                }
                
                AddEntry(option, searchList);
            }
            
            return searchList;
        }

        public bool OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
        {
            _callback?.Invoke((string) searchTreeEntry.userData);
            return true;
        }

        private void AddEntry(string option, List<SearchTreeEntry> entries)
        {
            var entry = new SearchTreeEntry(new GUIContent(option)){level = 1, userData = option};
            entries.Add(entry);
        }
    }
}