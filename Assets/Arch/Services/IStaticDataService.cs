using Assets.Arch.StaticData;
using Assets.Scripts;
using UnityEngine;

namespace Assets.Arch.Services
{
    public interface IStaticDataService : IService
    {
        void LoadCards();
        void LoadBonusCards();
        void LoadSettings();
        void LoadVisualSettings();
        void LoadBonusSettings();
        LevelData LoadLevel(int levelNumber);
        void LoadSounds();

        CardFacesStaticData CardFaces { get; }
        CardBacksStaticData CardBacks { get; }
        BonusCardsStaticData BonusCards { get; }
        GlobalSettings GlobalSettings { get; }
        VisualSettings VisualSettings { get; }
        BonusSettings BonusSettings { get; }
        AudioClip GetSound(string name);
    }
}