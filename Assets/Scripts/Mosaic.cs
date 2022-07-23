using Assets.Arch.Utilities;
using System.Linq;
using UnityEngine;

public class MosaicCardData
{
    public int X;
    public int Y;
    public int Index;

    public MosaicCardData(int x, int y, int index)
    {
        X = x;
        Y = y;
        Index = index;
    }
}

public class Mosaic : MonoBehaviour
{
    [SerializeField] private Sprite[] _mosaic;

    public void Create()
    {
        var _mosaicCard = Resources.Load("Prefabs/Grid/MosaicCard") as GameObject;

        _mosaic = Resources.LoadAll<Sprite>("Forest");

        var XSize = 8;
        var YSize = 8;

        var _grid = new GGrid<MosaicCardData>(XSize, YSize);

        for (var x = 0; x < XSize; x++)
            for (var y = 0; y <YSize; y++)
            {
                var index = _grid.CoordsToIndex(x, y);
                _grid.Cells[index] = new MosaicCardData(x, y, index);
            }


       Sprite[] m = _mosaic.Reverse().ToArray();

        var camXPos = (_grid.XSize + (_grid.XSize - 1) * 0f) / 2f - 0.5f;
        var camYPos = (_grid.YSize + (_grid.YSize - 1) * 0f) / 2f - 0.5f;
        Camera.main.transform.position = new Vector3(camXPos, camYPos, -10f); // Подумать куда девать

        var counter = _grid.XSize - 1;
        var mm = Shuffle(m);

        for (var x = 0; x < _grid.XSize; x++)
        {
            for (var y = 0; y < _grid.YSize; y++)
            {
                var go = Instantiate(_mosaicCard, new Vector3(counter, y), Quaternion.identity);
                go.name = x + " " + y;
                go.GetComponent<SpriteRenderer>().sprite = mm[_grid.CoordsToIndex(x, y)];
            }
            counter--;
        }
    }

    private Sprite[] Shuffle(Sprite[] array)
    {
        var rng = new System.Random();
        var n = array.Length;

        while (n > 1)
        {
            var k = rng.Next(n--);
            (array[n], array[k]) = (array[k], array[n]);
        }

        return array;
    }
}
