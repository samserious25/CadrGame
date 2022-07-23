using UnityEngine;

namespace Assets.Arch.StaticData
{
    [CreateAssetMenu(fileName = "BonusSettings", menuName = "StaticData/BonusSettings", order = 5)]
    public class BonusSettings : ScriptableObject
    {
        public int CorrectMatchesCountForReduceBonus = 2;
        public float OpenAllCloseDelay = 2f;
        public int TapCountToDeleteBonusCard;
    }
}