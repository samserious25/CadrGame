using System.Collections.Generic;
using UnityEngine;

namespace Assets.Arch.StaticData
{
    [CreateAssetMenu(fileName = "CardBacks", menuName = "StaticData/CardBacks", order = 2)]
    public class CardBacksStaticData : ScriptableObject
    {
        public List<Texture2D> Textures;
    }
}