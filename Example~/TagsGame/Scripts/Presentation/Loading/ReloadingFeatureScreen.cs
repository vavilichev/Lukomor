using System.Collections;
using Lukomor.Application.Signals;
using Lukomor.DIContainer;
using Lukomor.Example.Application.TagsGrid.Signals;
using Lukomor.Example.Domain;
using UnityEngine;
using VavilichevGD.Tools.Async;

namespace Lukomor.Example.Presentation.Loading
{
    public class ReloadingFeatureScreen : MonoBehaviour, ITagsGridRebuildStartSignalObserver, ITagsGridRebuiltSignalObserver
    {
        [SerializeField] private GameObject _goContent;
        
        private static ReloadingFeatureScreen _instance;
        
        private readonly DIVar<ISignalTower> _signalTower = new DIVar<ISignalTower>();

        private bool inClosingProces = false;

        private async void Start()
        {
            if (CreateSingleton())
            {
                await UnityAwaiters.WaitUntil(() => TagsGame.ProjectContext.IsReady);
                
                _signalTower.Value.Register<TagsGridRebuildStartSignal>(this);
                _signalTower.Value.Register<TagsGridRebuiltSignal>(this);
            }
        }

        private bool CreateSingleton()
        {
            var singletonCreated = false;

            if (_instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                singletonCreated = true;
                _instance = this;

                DontDestroyOnLoad(gameObject);
            }

            return singletonCreated;
        }

        private void OnDestroy()
        {
            _signalTower.Value.Unregister<TagsGridRebuildStartSignal>(this);
            _signalTower.Value.Unregister<TagsGridRebuiltSignal>(this);
        }
        
        public void ReceiveSignal(TagsGridRebuildStartSignal evt)
        {
            _goContent.SetActive(true);
        }
        
        public void ReceiveSignal(TagsGridRebuiltSignal evt)
        {
            if (!inClosingProces)
            {
                StartCoroutine(CloseLoadingScreenWithDelay(1f));
            }
        }

        private IEnumerator CloseLoadingScreenWithDelay(float delay)
        {
            inClosingProces = true;
            
            yield return new WaitForSeconds(1f);

            _goContent.SetActive(false);
            
            inClosingProces = false;
        }
    }
}