using Lukomor;
using Lukomor.DI;
using Lukomore.Example.TicTacToe.Gameplay;
using UnityEngine;
using UnityEngine.Serialization;

namespace Lukomore.Example.TicTacToe.View.Gameplay
{
    public class GameplayFeatureInstaller : MonoInstaller
    {
        [SerializeField] private GameFieldView gameFieldViewPrefab;
        
        public override void InstallBindings(IDIContainer container)
        {
            var gameplayFeature = new GameplayFeature();
            var gameFieldView = Instantiate(gameFieldViewPrefab);
            
            gameFieldView.Init(gameplayFeature.Field);
        }
    }
}