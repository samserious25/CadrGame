using Assets.Arch.ObjectPooling;
using UnityEngine;

namespace Assets.Arch.Factories
{
    public interface ISpawnFactory: IService
    {
        IObjectPool ObjectPool { get; }
        GameManager SpawnGameManager();
        GameObject SpawnCard(Vector2 position, Transform parent = null);
        GameObject SpawnTile(Vector2 position);
        SoundPlayer SpawnSoundPlayer();
        void ReturnToPool(GameObject gameObject);
    }
}
