namespace Assets.Scripts
{
    public struct MapData
    {
        public int XSize { get; }
        public int YSize { get; }
        public int DifferentPicturesSize { get; }
        public int CountToMatch { get; }
        public int NotNullTileCount { get; }

        public MapData(int xSize, int ySize, int differentPicturesCount, int countToMatch, int notNullTileCount)
        {
            XSize = xSize;
            YSize = ySize;
            DifferentPicturesSize = differentPicturesCount;
            CountToMatch = countToMatch;
            NotNullTileCount = notNullTileCount;
        }
    }
}