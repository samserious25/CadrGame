using Assets.Arch.ObjectPooling;
using UnityEngine;

namespace Assets.Arch.Providers
{
    public interface IAssets : IService
    {
        GameObject Instantiate(string path);
        GameObject InstantiatePooled(string path, Vector3 position, IObjectPool pool, Transform parent = null, bool autoRecycle = false,
            float timeToRecycle = 0);
        GameObject DestroyToPool(GameObject gameObject, IObjectPool pool);
        AudioClip GetAudioClip(string path);
    }
}