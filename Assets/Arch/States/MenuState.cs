using Assets.Arch.Services;
using Assets.Arch.Utilities;
using UnityEngine;

namespace Assets.Arch.States
{
    public class MenuState : IState
    {
        private readonly StateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly LoadingScreen _loadingScreen;

        public MenuState(StateMachine stateMachine, SceneLoader sceneLoader, LoadingScreen loadingScreen)
        {
            _gameStateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _loadingScreen = loadingScreen;
        }

        public void Enter()
        {
            _sceneLoader.Load("Menu", OnSceneLoad);
        }

        private void OnSceneLoad()
        {
           _loadingScreen.Hide(0f);
            Logging.LogInfo("Menu scene loaded");

            var staticData = GameServices.Container.Get<IStaticDataService>();
            staticData.LoadSettings();
            staticData.LoadVisualSettings();

            Application.targetFrameRate = staticData.GlobalSettings.FpsLimit;

            //_gameStateMachine.Enter<GameState>();
        }

        public void Exit()
        {
            
        }
    }
}
