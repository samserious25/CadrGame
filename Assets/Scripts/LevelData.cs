namespace Assets.Scripts
{
    [System.Serializable]
    public class LevelData
    {
        public int XSize;
        public int YSize;
        public string NullCells;
        public int CountToMatch;
        public string Pictures;
        public int CardBack;
        public int BonusCard;
        public int TapBonusBurns;
    }

    [System.Serializable]
    public class AllLevels
    {
        public LevelData[] data;
    }
}