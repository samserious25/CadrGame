using UnityEngine;

namespace Assets.Arch.ObjectPooling
{
    public interface IPool
    {
        void CreatePool(int count);
        GameObject CreateObject(bool isActiveByDefault = false);
        bool HasFreeElement(out GameObject element);
        GameObject Get();
        void Return(GameObject createdObject);
    }
}