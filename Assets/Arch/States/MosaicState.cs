using Assets.Arch.Factories;
using Assets.Arch.Services;
using Assets.Arch.Utilities;
using Assets.Scripts;
using UnityEngine;

namespace Assets.Arch.States
{
    public class MosaicState : IState
    {
        private readonly StateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly LoadingScreen _loadingScreen;
        private readonly ISpawnFactory _spawnFactory;

        public MosaicState(StateMachine stateMachine, SceneLoader sceneLoader, LoadingScreen loadingScreen, ISpawnFactory spawnFactory)
        {
            _gameStateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _loadingScreen = loadingScreen;
            _spawnFactory = spawnFactory;
        }

        public void Enter()
        {
            _loadingScreen.Show();
            _sceneLoader.Load("Mosaic", OnSceneLoad);
        }

        private void OnSceneLoad()
        {
            _loadingScreen.Hide(2f);
            Logging.LogInfo("Game scene loaded");

            var staticData = GameServices.Container.Get<IStaticDataService>();
            staticData.LoadSettings();
            staticData.LoadVisualSettings();

            Application.targetFrameRate = staticData.GlobalSettings.FpsLimit;

            CreateMosaic();
        }

        private void CreateMosaic()
        {
            var mosaic = new GameObject("Mosaic");
            var mosaicComponent = mosaic.AddComponent<Mosaic>();
            mosaicComponent.Create();
        }

        public void Exit()
        {

        }
    }
}