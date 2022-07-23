using UnityEngine;

namespace Assets.Arch
{
    [CreateAssetMenu(fileName = "GlobalSettings", menuName = "StaticData/GlobalSettings", order = 1)]
    public class GlobalSettings : ScriptableObject
    {
        public int FpsLimit = 60;
    }
}
