using Assets.Arch.Factories;
using Assets.Arch.Utilities;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public class ClassicLogic : MonoBehaviour, IGameLogic
    {
        private VisualSettings _visualSettings;
        private Queue<CardData> _matches;
        private float _offset;
        private GGrid<CardData> _grid;
        private int _countToMatch;
        private ISpawnFactory _spawnFactory;
        private List<CardData> _cards;
        private int _filledTiles;
        private CardData _bonusCardData;
        private CardData _tappedCardData;

        public event Action<string> PlaySound;
        public event Action LevelCompleted;
        public event Action CorrectMatch;
        public event Action WrongMatch;
        public event Action<int> BonusFound;
        public event Action Tap;

        private bool _canPlay;
        private GridCreator _gridCreator;

        private void OnDisable() =>
            _gridCreator.CanPlay -= OnGridCreated;

        public void Construct(ISpawnFactory spawnFactory, VisualSettings visualSettings, float offset, GridCreator gridCreator)
        {
            _spawnFactory = spawnFactory;
            _visualSettings = visualSettings;
            _offset = offset;
            _gridCreator = gridCreator;

            _matches = new Queue<CardData>();
            _cards = new List<CardData>();
        }

        public void StartLevel(GGrid<CardData> grid, int countToMatch, CardData bonusCard)
        {
            _grid = grid;
            _countToMatch = countToMatch;
            _filledTiles = grid.Cells.Where(x => x != null).Count();
            _gridCreator.CanPlay += OnGridCreated;
            _bonusCardData = bonusCard;
        }

        private void OnGridCreated() =>
            _canPlay = true;

        private void Update()
        {
            if (_canPlay && Input.GetMouseButtonDown(0))
                ClickTile();
        }

        private void ClickTile()
        {
            var coords = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            var x = Mathf.RoundToInt(coords.x - coords.x * _offset);
            var y = Mathf.RoundToInt(coords.y - coords.y * _offset);

            if (!_grid.AreCoordsValid(x, y))
                return;

            var index = _grid.CoordsToIndex(x, y);

            _tappedCardData = _grid.Cells[index];

            if (!_grid.IsIndexValid(index))
                return;

            if (_grid.Cells[index] == null || !_grid.Cells[index].CanClick)
                return;

            _grid.Cells[index].CanClick = false;

            PlaySound?.Invoke("Click");
            Tap?.Invoke();

            RotateCard(index);
        }

        private void RotateCard(int index)
        {
            if (_grid.Cells[index].Bonus >= 0)
            {
                RotateBonusCard(index);
                return;
            }

            _grid.Cells[index].CardTransform.DORotate(new Vector3(0, 180, 0), _visualSettings.CardRotateTime, RotateMode.LocalAxisAdd)
                .SetEase(Ease.InOutSine).OnComplete(() => CheckMatches(index));
        }

        private void RotateBonusCard(int index)
        {
            _canPlay = false;
            _filledTiles--;

            BonusFound?.Invoke(_grid.Cells[index].Bonus);
            BurnOutWithScale(_grid.Cells[index], () => FadeOutCard(index, 0f));
        }

        private void CheckMatches(int index)
        {
            _matches.Enqueue(_grid.Cells[index]);

            if (_matches.Count == 2)
                if (!Equal(_cards, 2))
                {
                    foreach (var card in _cards)
                    {
                        if (card.Bonus < 0)
                            RotateCardToIdle(card.Index);
                    }

                    _cards.Clear();
                    WrongMatch?.Invoke();
                    PlaySound?.Invoke("ErrorMatch");
                    return;
                }

            if (_matches.Count < _countToMatch - _cards.Count)
                return;

            var equal = Equal(_cards, _countToMatch - _cards.Count);

            if (equal)
            {
                foreach (var card in _cards)
                    FadeOutCard(card.Index);

                _filledTiles -= _countToMatch;

                _cards.Clear();

                CorrectMatch?.Invoke();
                PlaySound?.Invoke("Match");
            }
            else
            {
                foreach (var card in _cards)
                    RotateCardToIdle(card.Index);

                _cards.Clear();

                WrongMatch?.Invoke();
                PlaySound?.Invoke("ErrorMatch");
            }
        }

        public void OpenMatch(CardData clickedCard)
        {
            var matchValue = clickedCard.Value;
            var cardsWithValue = _grid.Cells.Where(x => x != null && x.CanClick == true && x.Value == matchValue).ToList();

            var match = new List<CardData>();
            match.Add(cardsWithValue.Find(x => x == clickedCard));

            for (int i = 0; i < cardsWithValue.Count; i++)
            {
                if (cardsWithValue[i] == clickedCard)
                    continue;

                if (match.Count < _countToMatch)
                    match.Add(cardsWithValue[i]);
            }

            for (int i = 0; i < match.Count; i++)
                RotateCard(match[i].Index);
        }

        public bool DeleteBonus()
        {
            if (_bonusCardData == null)
                return false;

            var bonus = _grid.Cells.FirstOrDefault(x => x != null && x.Bonus >= 0 && x.CanClick);

            if (bonus == null)
                return false;

            if (_tappedCardData != null && _tappedCardData.Bonus >= 0)
                return false;

            _canPlay = false;

            bonus.CanClick = false;

            _bonusCardData = null;
            _filledTiles--;

            BurnOutWithScale(bonus, () => FadeOutCard(bonus.Index, 0f));

            return true;
        }

        public void OpenAllCards(float closeDelay)
        {
            _canPlay = false;

            for (int i = 0; i < _grid.Cells.Length; i++)
            {
                if (_grid.Cells[i] == null)
                    continue;

                Sequence mySequence = DOTween.Sequence();

                mySequence
                    .Append(_grid.Cells[i].CardTransform.DORotate(new Vector3(0, 180, 0), _visualSettings.CardRotateTime, RotateMode.LocalAxisAdd)
                    .SetEase(Ease.InOutSine))
                    .PrependInterval(closeDelay)
                    .Insert(0, _grid.Cells[i].CardTransform.DORotate(new Vector3(0, 180, 0), _visualSettings.CardRotateTime, RotateMode.LocalAxisAdd)
                    .SetEase(Ease.InOutSine))
                    .OnComplete(() => _canPlay = true);
            }
        }

        private void RotateCardToIdle(int index)
        {
            _grid.Cells[index].CardTransform.DORotate(new Vector3(0, 180, 0), _visualSettings.CardRotateTime, RotateMode.LocalAxisAdd)
                .SetEase(Ease.InOutSine).OnComplete(() =>
                {
                    _grid.Cells[index].CanClick = true;
                });
        }

        private bool Equal(List<CardData> cards, int countToCheck)
        {
            for (var i = 0; i < countToCheck; i++)
                cards.Add(_matches.Dequeue());

            var first = cards.First();
            var equal = cards.All(x => x.Value == first.Value);

            return equal;
        }

        private void BurnOutWithScale(CardData cardData, Action OnCompleteMethod)
        {
            Sequence mySequence = DOTween.Sequence();

            cardData.CardTransform.position = new Vector3(cardData.CardTransform.position.x, cardData.CardTransform.position.y, -1f);

            mySequence
                .Append(cardData.CardTransform.DOScale(new Vector3(2f, 2f), _visualSettings.BonusShowWithRotateTime))
                .Insert(0, cardData.CardTransform.DORotate(new Vector3(0, 180, 0), mySequence.Duration(), RotateMode.LocalAxisAdd).OnComplete(() =>
                {
                    _canPlay = true;
                    FadeOutCard(cardData.Index);
                }))
                .SetEase(Ease.InOutSine);
        }

        private void FadeOutCard(int index, float delay = 0f)
        {
            float dissolveLevel = 0;
            float edgeWidth = 0;

            DOTween.To(() => edgeWidth, x => edgeWidth = x, _visualSettings.DissolveEdgeWidth, _visualSettings.CardFadeOutTime)
            .OnUpdate(() =>
            {
                _grid.Cells[index].CardRenderer.material.SetFloat("_Edges", edgeWidth);
            })
            .SetDelay(delay);

            DOTween.To(() => dissolveLevel, x => dissolveLevel = x, 1, _visualSettings.CardFadeOutTime)
            .OnUpdate(() =>
            {
                _grid.Cells[index].CardRenderer.material.SetFloat("_Level", dissolveLevel);
            })
            .SetEase(Ease.Linear)
            .SetDelay(delay)
            .OnComplete(() =>
            {
                _spawnFactory.ReturnToPool(_grid.Cells[index].CardTransform.gameObject);
                _grid.Cells[index].CardRenderer.material.SetFloat("_Edges", 0f);
                _grid.Cells[index].CardRenderer.material.SetFloat("_Level", 0f);
                _grid.Cells[index].CardTransform.DOScale(Vector3.one, 0f);
                _grid.Cells[index].CardTransform.position = new Vector3(_grid.Cells[index].CardTransform.position.x, _grid.Cells[index].CardTransform.position.y, 0f);
                StartCoroutine(CheckLevelCompleted());
            });
        }

        private IEnumerator CheckLevelCompleted()
        {
            yield return null;
            yield return null;
            yield return null;

            if (_filledTiles == 0)
            {
                _canPlay = false;
                LevelCompleted?.Invoke();
            }
        }
    }
}