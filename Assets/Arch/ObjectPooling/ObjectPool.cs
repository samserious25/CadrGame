using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Arch.Services;
using UnityEngine;

namespace Assets.Arch.ObjectPooling
{
    public class ObjectPool : IObjectPool
    {
        private readonly Dictionary<string, Pool> _pools = new Dictionary<string, Pool>();
        private Transform _poolParent;
        private readonly ICoroutineRunner _coroutineRunner;

        public ObjectPool(ICoroutineRunner coroutineRunner) =>
            _coroutineRunner = coroutineRunner;

        public GameObject Get(GameObject prefab, bool autoRecycle = true, float timeToRecycle = 2f)
        {
            if (_poolParent == null)
                _poolParent = new GameObject("ObjectPool").transform;

            if (!_pools.Keys.Contains(prefab.name))
            {
                var poolParent = new GameObject(prefab.name + "_Pool");
                poolParent.transform.SetParent(_poolParent);
                _pools.Add(prefab.name, new Pool(prefab, 1, poolParent.transform));
            }

            var created = _pools[prefab.name].Get();

            if (autoRecycle)
                _coroutineRunner.StartCoroutine(DeleteRoutine(created, prefab, timeToRecycle));

            return created;
        }

        public GameObject Return(GameObject createdObject, GameObject prefab = null) //Над этим местом надо еще подумать
        {
            if (prefab != null)
            {
                if (!_pools.Keys.Contains(prefab.name))
                {
                    var poolParent = new GameObject(prefab.name + "_Pool");
                    poolParent.transform.SetParent(_poolParent);
                    _pools.Add(prefab.name, new Pool(prefab, 0, poolParent.transform));
                }
            }
            else
            {
                var createdName = createdObject.name.Contains("(Clone)")
                    ? createdObject.name.Remove(createdObject.name.Length - 7)
                    : createdObject.name;

                if (!_pools.Keys.Contains(createdName))
                {
                    var poolParent = new GameObject(createdName + "_Pool");
                    poolParent.transform.SetParent(_poolParent);
                    _pools.Add(createdName, new Pool(createdObject, 0, poolParent.transform));
                }
            }

            var go = prefab == null ? createdObject.name : prefab.name;
            var properName = go.Contains("(Clone)") ? go.Remove(createdObject.name.Length - 7) : go;
            createdObject.transform.SetParent(_pools[properName].Container);
            createdObject.SetActive(false);
            return createdObject;
        }

        private IEnumerator DeleteRoutine(GameObject createdObject, GameObject prefab, float timeToRecycle)
        {
            yield return new WaitForSeconds(timeToRecycle);
            Return(createdObject, prefab);
        }
    }
}