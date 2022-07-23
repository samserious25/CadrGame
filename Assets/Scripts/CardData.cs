using UnityEngine;

namespace Assets.Scripts
{
    public class CardData
    {
        public Transform CardTransform { get; }
        public Renderer CardRenderer { get; }
        public int Value { get; }
        public bool CanClick { get; set; }
        public int Index { get; }
        public int Bonus { get; set; }

        public CardData(Transform cardTransform, Renderer cardRenderer, int value, int index, bool canClick, int bonus)
        {
            CardTransform = cardTransform;
            CardRenderer = cardRenderer;
            Value = value;
            Index = index;
            CanClick = canClick;
            Bonus = bonus;
        }
    }
}