using System.Collections.Generic;
using UnityEngine;

namespace Assets.Arch.StaticData
{
    [CreateAssetMenu(fileName = "CardFaces", menuName = "StaticData/CardFaces", order = 1)]
    public class CardFacesStaticData : ScriptableObject
    {
        public List<Texture2D> Textures;
    }
}
