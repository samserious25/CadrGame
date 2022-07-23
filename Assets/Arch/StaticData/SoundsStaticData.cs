using System.Collections.Generic;
using UnityEngine;

namespace Assets.Arch.StaticData
{
    [CreateAssetMenu(fileName = "Sounds", menuName = "StaticData/SoundsStaticData", order = 4)]
    public class SoundsStaticData : ScriptableObject
    {
        public List<AudioClip> GameSounds;
    }
}