using Assets.Arch.Factories;
using Assets.Arch.StaticData;
using Assets.Arch.Utilities;
using DG.Tweening;
using System;
using UnityEngine;

namespace Assets.Scripts
{
    public class GridCreator
    {
        public event Action CanPlay;
        private readonly ISpawnFactory _spawnFactory;
        private readonly CardFacesStaticData _cardFaces;
        private readonly CardBacksStaticData _cardBacks;
        private readonly BonusCardsStaticData _bonusCards;
        private readonly BonusSettings _bonusSettings;
        private float _offset;
        private Transform tileParent;

        public GridCreator(ISpawnFactory spawnFactory, CardFacesStaticData cardFaces, CardBacksStaticData cardBacks, BonusCardsStaticData bonusCards, BonusSettings bonusSettings)
        {
            _spawnFactory = spawnFactory;
            _cardFaces = cardFaces;
            _cardBacks = cardBacks;
            _bonusCards = bonusCards;
            _bonusSettings = bonusSettings;
        }

        private int[] GenerateMatches(int notNullTileCount, int countToMatch, int[] pictures, int bonus)
        {
            var matchSize = countToMatch * 2 - 1;
            var result = new int[notNullTileCount];
            
            var counter = 0;

            var maxSizeWithBonuses = bonus >= 0 ? notNullTileCount - 1 : notNullTileCount;

            for (var i = 0; i < maxSizeWithBonuses; i++)
            {
                if (matchSize == 0)
                    matchSize = countToMatch;

                if (counter == pictures.Length)
                    counter = 0;

                result[i] = pictures[counter];

                if (matchSize == countToMatch)
                    counter++;

                matchSize--;
            }

            if (bonus >= 0)
                result[^1] = bonus + 1000;

            Shuffle(result);
            return result;
        }

        public void Shuffle<T>(T[] arr)
        {
            var rand = new System.Random();

            for (int i = arr.Length - 1; i >= 1; i--)
            {
                int j = rand.Next(i + 1);

                T tmp = arr[j];
                arr[j] = arr[i];
                arr[i] = tmp;
            }
        }

        public void ClearGrid()
        {
            if (tileParent != null && tileParent.childCount > 0)
                for (int i = 0; i < tileParent.childCount; i++)
                    _spawnFactory.ReturnToPool(tileParent.GetChild(i).gameObject);

            tileParent = null;
        }

        public GGrid<CardData> Create(GridData gridData, float offset, Transform parent = null)
        {
            _offset = offset + 1;

            _bonusSettings.TapCountToDeleteBonusCard = gridData.TapBonusBurns;

            var _grid = new GGrid<CardData>(gridData.XSize, gridData.YSize);

            var camXPos = (gridData.XSize + (gridData.XSize - 1) * offset) / 2f - 0.5f;
            var camYPos = (gridData.YSize + (gridData.YSize - 1) * offset) / 2f - 0.5f;

            Camera.main.transform.position = new Vector3(camXPos, camYPos, -10f); // Подумать куда девать

            var randomNumbers = GenerateMatches(gridData.NotNullTileCount, gridData.CountToMatch, gridData.Pictures, gridData.BonusCard);

            var counter = 0;

            for (var x = 0; x < gridData.XSize; x++)
                for (var y = 0; y < gridData.YSize; y++)
                {
                    var index = _grid.CoordsToIndex(x, y);

                    if (gridData.NullData != null && gridData.NullData.Contains(index))
                        continue;

                    var tile = _spawnFactory.SpawnTile(new Vector2(x, y) * _offset);

                    if (tileParent == null)
                        tileParent = tile.transform.parent;

                    var card = _spawnFactory.SpawnCard(new Vector2(x, y) * _offset, parent);
                    card.transform.rotation = Quaternion.identity;

                    var cardRenderer = card.GetComponent<Renderer>();

                    var properTexture = randomNumbers[counter] >= 1000
                        ? _bonusCards.Textures[randomNumbers[counter] - 1000]
                        : _cardFaces.Textures[randomNumbers[counter]];

                    cardRenderer.material.SetTexture("_MainTex", properTexture);
                    cardRenderer.material.SetTexture("_BackTex", _cardBacks.Textures[gridData.CardBack]);

                    var bonusValue = randomNumbers[counter] >= 1000 ? gridData.BonusCard : -1;

                    var cardData = new CardData(card.transform, cardRenderer, randomNumbers[counter], index, true, bonusValue);
                    _grid.SetValue(x, y, cardData);

                    counter++;
                }

            parent.transform.position += new Vector3(0f, 20f);
            parent.transform.DOMove(Vector3.zero, 1f).SetEase(Ease.InOutSine).OnComplete(() => CanPlay?.Invoke());

            return _grid;
        }
    }
}