using Assets.Arch.ObjectPooling;
using UnityEngine;

namespace Assets.Arch.Providers
{
    public class AssetProvider : IAssets
    {
        public GameObject Instantiate(string path)
        {
            var prefab = Resources.Load<GameObject>(path);
            return Object.Instantiate(prefab);
        }

        public GameObject InstantiatePooled(string path, Vector3 position, IObjectPool pool, Transform parent = null, bool autoRecycle = false,
            float timeToRecycle = 0)
        {
            var prefab = Resources.Load<GameObject>(path);
            var go = pool.Get(prefab, autoRecycle, timeToRecycle);
            go.transform.position = position;
            if (parent != null)
                go.transform.SetParent(parent);
            return go;
        }

        public GameObject DestroyToPool(GameObject gameObject, IObjectPool pool) =>
            pool.Return(gameObject);

        public AudioClip GetAudioClip(string path) =>
            Resources.Load<AudioClip>(path);
    }
}
