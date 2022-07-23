using Assets.Arch.ObjectPooling;
using Assets.Arch.Providers;
using Assets.Scripts;
using UnityEngine;

namespace Assets.Arch.Factories
{
    public class SpawnFactory : ISpawnFactory
    {
        public IAssets Assets { get; }
        public IObjectPool ObjectPool { get; }

        public SpawnFactory(IAssets assets, IObjectPool objectPool)
        {
            Assets = assets;
            ObjectPool = objectPool;
        }

        public GameManager SpawnGameManager() =>
            Assets.Instantiate(AssetPath.GameManagerPath).GetComponent<GameManager>();

        public GameObject SpawnCard(Vector2 position, Transform parent = null) =>
            Assets.InstantiatePooled(AssetPath.CardPath, position, ObjectPool, parent);

        public GameObject SpawnTile(Vector2 position) =>
            Assets.InstantiatePooled(AssetPath.TilePath, position, ObjectPool);

        public SoundPlayer SpawnSoundPlayer() =>
             Assets.Instantiate(AssetPath.SoundPlayerPath).GetComponent<SoundPlayer>();

        public void ReturnToPool(GameObject gameObject) =>
            Assets.DestroyToPool(gameObject, ObjectPool);
    }
}
