using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Arch.ObjectPooling
{
    public class Pool : IPool
    {
        private readonly GameObject _prefab;
        private readonly bool _autoExpand;
        public Transform Container { get; }
        private List<GameObject> _pool;

        public Pool(GameObject prefab, int count, Transform container, bool autoExpand = true)
        {
            _prefab = prefab;
            _autoExpand = autoExpand;
            Container = container;

            CreatePool(count);
        }

        public void CreatePool(int count)
        {
            _pool = new List<GameObject>();

            for (var i = 0; i < count; i++)
                CreateObject();
        }

        public GameObject CreateObject(bool isActiveByDefault = false)
        {
            var createdObject = Object.Instantiate(_prefab, Container);
            createdObject.SetActive(isActiveByDefault);
            _pool.Add(createdObject);
            return createdObject;
        }

        public bool HasFreeElement(out GameObject element)
        {
            foreach (var t in _pool)
                if (!t.activeInHierarchy)
                {
                    element = t.gameObject;
                    element.SetActive(true);
                    return true;
                }

            element = null;
            return false;
        }

        public GameObject Get()
        {
            if (HasFreeElement(out var element))
                return element;

            if (_autoExpand)
                return CreateObject(true);

            throw new Exception($"There is no free elements in pool of type {typeof(GameObject)}");
        }

        public void Return(GameObject createdObject)
        {
            createdObject.transform.SetParent(Container);
            createdObject.SetActive(false);
        }
    }
}