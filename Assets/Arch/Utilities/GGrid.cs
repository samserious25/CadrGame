using System;
using System.Collections.Generic;

namespace Assets.Arch.Utilities
{
    public class GGrid<T>
    {
        public T[] Cells { get; }
        public int XSize { get; }
        public int YSize { get; }
        //public float XOffset { get; }
        //public float YOffset { get; }

        public GGrid(int xSize, int ySize)
        {
            Cells = new T[xSize * ySize];
            XSize = xSize;
            YSize = ySize;
            //XOffset = XSize / 2f - 0.5f;
            //YOffset = YSize / 2f - 0.5f;
        }

        public (int x, int y) GetCoords(T value)
        {
            var index = Array.IndexOf(Cells, value);
            return GetCoords(index);
        }

        public int GetIndex(T value)
        {
            var coords = GetCoords(value);
            return CoordsToIndex(coords.Item1, coords.Item2);
        }

        public int CoordsToIndex(int x, int y) =>
            y * XSize + x;

        public (int x, int y) GetCoords(int index) =>
            new(index % XSize, index / XSize);

        public void SetValue(int x, int y, T value) =>
            Cells[CoordsToIndex(x, y)] = value;

        public void SetValue(int index, T value) =>
            Cells[index] = value;

        public T GetValue(int x, int y) =>
            Cells[CoordsToIndex(x, y)];

        public T GetValue(int index) =>
           Cells[index];

        public bool AreCoordsValid(int x, int y) =>
            (x >= 0 && x < XSize && y >= 0 && y < YSize);

        public bool IsIndexValid(int index) =>
            index < Cells.Length && index >= 0;

        public List<T> GetNeighborsAround(int x, int y, int radius = 1)
        {
            var neighbors = new List<T>();
            var self = CoordsToIndex(x, y);

            for (var i = x - radius; i <= x + radius; i++)
                for (var j = y - radius; j <= y + radius; j++)
                    if (AreCoordsValid(i, j))
                    {
                        var neighbor = CoordsToIndex(i, j);
                        if (neighbor != self)
                            neighbors.Add(GetValue(i, j));
                    }

            return neighbors;
        }
    }
}