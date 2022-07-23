using Assets.Arch.Factories;
using Assets.Arch.Services;
using Assets.Arch.Utilities;
using Assets.Scripts;
using UnityEngine;

namespace Assets.Arch.States
{
    public class GameState : IState
    {
        private readonly StateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly LoadingScreen _loadingScreen;
        private readonly ISpawnFactory _spawnFactory;

        public GameState(StateMachine stateMachine, SceneLoader sceneLoader, LoadingScreen loadingScreen, ISpawnFactory spawnFactory)
        {
            _gameStateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _loadingScreen = loadingScreen;
            _spawnFactory = spawnFactory;
        }

        public void Enter()
        {
            _loadingScreen.Show();
            _sceneLoader.Load("GameScene", OnSceneLoad);
        }

        private void OnSceneLoad()
        {
            _loadingScreen.Hide(2f);
            Logging.LogInfo("Game scene loaded");

            var staticData = GameServices.Container.Get<IStaticDataService>();
            staticData.LoadSettings();
            staticData.LoadVisualSettings();

            Application.targetFrameRate = staticData.GlobalSettings.FpsLimit;

            staticData.LoadCards();
            staticData.LoadBonusCards();
            staticData.LoadSounds();
            staticData.LoadBonusSettings();

            CreateGrid(staticData);
        }

        private void CreateGrid(IStaticDataService staticData)
        {
            var gameManager = _spawnFactory.SpawnGameManager();

            var gridLoader = new GridLoader(staticData);
            var gridCreator = new GridCreator(_spawnFactory, staticData.CardFaces, staticData.CardBacks, staticData.BonusCards, staticData.BonusSettings);

            var logic = gameManager.gameObject.AddComponent<ClassicLogic>();

            gameManager.Construct(logic, staticData, _spawnFactory, 0.1f, gridLoader, gridCreator);
            gameManager.gameObject.AddComponent<BonusManager>().Construct(logic, staticData.BonusSettings);

            CreateSounds(staticData, logic);

            gameManager.StarLevel(2);
        }

        private void CreateSounds(IStaticDataService staticData, IGameLogic logic)
        {
            var soundPlayer = _spawnFactory.SpawnSoundPlayer();
            soundPlayer.Construct(staticData, logic);
        }

        public void Exit()
        {

        }
    }
}