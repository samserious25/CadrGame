using System.Collections.Generic;

namespace Assets.Scripts
{
    public struct GridData
    {
        public int XSize { get; }
        public int YSize { get; }
        public int[] Pictures { get; }
        public int CountToMatch { get; }
        public int NotNullTileCount { get; }
        public List<int> NullData { get; }
        public int CardBack { get; }
        public int BonusCard { get; }
        public int  TapBonusBurns { get; }

        public GridData(int xSize, int ySize, int[] pictures, int countToMatch, int notNullTileCount, List<int> nullData, int cardBack, int bonusCard, int tapBonusBurns)
        {
            XSize = xSize;
            YSize = ySize;
            Pictures = pictures;
            CountToMatch = countToMatch;
            NotNullTileCount = notNullTileCount;
            NullData = nullData;
            CardBack = cardBack;
            BonusCard = bonusCard;
            TapBonusBurns = tapBonusBurns;
        }
    }
}