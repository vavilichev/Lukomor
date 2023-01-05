using System.Collections;
using Lukomor.Application;
using Lukomor.Common.DIContainer;
using Lukomor.Domain.Signals;
using Lukomor.TagsGame.TagsGrid.Signals;
using UnityEngine;

namespace Lukomor.TagsGame.Loading.Presentation
{
    public class ReloadingFeatureScreen : MonoBehaviour, ITagsGridRebuildStartSignalObserver, ITagsGridRebuiltSignalObserver
    {
        [SerializeField] private GameObject _goContent;
        
        private static ReloadingFeatureScreen _instance;
        
        private readonly DIVar<ISignalTower> _signalTower = new DIVar<ISignalTower>();

        private bool inClosingProces = false;

        #region Unity lifecycle

        private void Start()
        {
            if (TryCreateSingleton())
            {
                if (!Game.IsMainObjectsBound)
                {
                    Game.ProjectContextPreInitialized += OnGameProjectContextPreInitialized;
                }
                else
                {
                    Init();
                }
            }
        }

        private void OnDestroy()
        {
            if (Game.IsMainObjectsBound)
            {
                _signalTower.Value.Unregister<TagsGridRebuildStartSignal>(this);
                _signalTower.Value.Unregister<TagsGridRebuiltSignal>(this);    
            }
        }
        
        #endregion

        #region Methods

        private bool TryCreateSingleton()
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

        private void Init()
        {
            _signalTower.Value.Register<TagsGridRebuildStartSignal>(this);
            _signalTower.Value.Register<TagsGridRebuiltSignal>(this);
        }

        private IEnumerator CloseLoadingScreenWithDelay(float delay)
        {
            inClosingProces = true;
            
            yield return new WaitForSeconds(1f);

            _goContent.SetActive(false);
            
            inClosingProces = false;
        }

        #endregion

        #region Events
        
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

        private void OnGameProjectContextPreInitialized()
        {
            Game.ProjectContextPreInitialized -= OnGameProjectContextPreInitialized;

            Init();
        }

        #endregion
       
    }
}