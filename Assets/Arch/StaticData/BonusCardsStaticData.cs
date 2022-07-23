using System.Collections.Generic;
using UnityEngine;

namespace Assets.Arch.StaticData
{
    [CreateAssetMenu(fileName = "BonusCards", menuName = "StaticData/BonusCardsStaticData", order = 6)]
    public class BonusCardsStaticData : ScriptableObject
    {
        public List<Texture2D> Textures;
    }
}