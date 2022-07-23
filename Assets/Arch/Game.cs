using Assets.Arch.Factories;
using Assets.Arch.ObjectPooling;
using Assets.Arch.Providers;
using Assets.Arch.Services;
using Assets.Arch.States;
using Assets.Arch.Utilities;
using UnityEngine;

namespace Assets.Arch
{
    public class Game: MonoBehaviour, ICoroutineRunner
    {
        public void Run(LoadingScreen loadingScreen)
        {
            var sceneLoader = new SceneLoader(this);
            var objectPool = new ObjectPool(this);
            var assets = new AssetProvider();
            var spawnFactory = new SpawnFactory(assets, objectPool);

            GameServices.Container.Register<ISpawnFactory>(spawnFactory);
            var stateMachine = new StateMachine(sceneLoader, loadingScreen, spawnFactory);
          
            GameServices.Container.Register(stateMachine);
            GameServices.Container.Register<IObjectPool>(objectPool);
            GameServices.Container.Register<IAssets>(assets);
            GameServices.Container.Register<IStaticDataService>(new StaticDataService());

            stateMachine.Enter<GameState>();
        }
    }
}
