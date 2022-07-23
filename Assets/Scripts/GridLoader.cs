using Assets.Arch.Services;
using System.Linq;

namespace Assets.Scripts
{
    public class GridLoader
    {
        private readonly IStaticDataService _staticData;

        public GridLoader(IStaticDataService staticData) =>
            _staticData = staticData;

        public GridData GetLevelData(int levelNumber)
        {
            var level = _staticData.LoadLevel(levelNumber);

            var xSize = level.XSize;
            var ySize = level.YSize;

            var picturesStr = level.Pictures.Split('-');
            int length = int.Parse(picturesStr[^1]);
            var pictures = new int[length];
            int counter = 0;
            for (int i = int.Parse(picturesStr[0]); i < length; i++)
            {
                pictures[counter] = i;
                counter++;
            }

            var countToMatch = level.CountToMatch;

            var nullDataString = string.IsNullOrEmpty(level.NullCells) ? null : level.NullCells.Split(',');
            var notNullTileCount = string.IsNullOrEmpty(level.NullCells) ? xSize * ySize : xSize * ySize - nullDataString.Length;

            var nullDataInt = string.IsNullOrEmpty(level.NullCells) ? null : new int[nullDataString.Length];

            if (nullDataString != null)
                for (int i = 0; i < nullDataString.Length; i++)
                    nullDataInt[i] = int.Parse(nullDataString[i]);

            var cardBack = level.CardBack;
            var bonusCard = level.BonusCard;

            var nullDataList = nullDataInt == null ? null : nullDataInt.ToList();
            var tapBonusBurns = level.TapBonusBurns;

            return new GridData(xSize, ySize, pictures, countToMatch, notNullTileCount, nullDataList, cardBack, bonusCard, tapBonusBurns);
        }
    }
}
