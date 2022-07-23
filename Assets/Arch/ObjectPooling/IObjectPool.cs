using UnityEngine;

namespace Assets.Arch.ObjectPooling
{
    public interface IObjectPool: IService
    {
        GameObject Get(GameObject prefab, bool autoRecycle = true, float timeToRecycle = 2f);
        GameObject Return(GameObject createdObject, GameObject prefab = null);
    }
}