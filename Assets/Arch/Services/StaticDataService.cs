using Assets.Arch.StaticData;
using Assets.Scripts;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Arch.Services
{
    public class StaticDataService : IStaticDataService
    {
        private const string GlobalSettingsDataPath = "StaticData/Settings/GlobalSettings";
        private const string VisualSettingsDataPath = "StaticData/Settings/VisualSettings";
        private const string BonusSettingsDataPath = "StaticData/Settings/BonusSettings";

        private const string CardFacesDataPath = "StaticData/CardData/CardFaces";
        private const string CardBacksDataPath = "StaticData/CardData/CardBacks";
        private const string BonusCardsDataPath = "StaticData/CardData/BonusCards";

        private const string LevelsDataPath = "StaticData/Levels";
        private const string SoundsDataPath = "StaticData/Sounds/Sounds";

        private Dictionary<string, AudioClip> _sounds;
        public CardFacesStaticData CardFaces { get; private set; }
        public CardBacksStaticData CardBacks { get; private set; }
        public GlobalSettings GlobalSettings { get; private set; }
        public VisualSettings VisualSettings { get; private set; }
        public BonusSettings BonusSettings { get; private set; }
        public BonusCardsStaticData BonusCards { get; private set; }

        public void LoadSounds()
        {
            _sounds = Resources
              .Load<SoundsStaticData>(SoundsDataPath).GameSounds
              .ToDictionary(x => x.name, x => x);
        }

        public AudioClip GetSound(string name) =>
          _sounds.TryGetValue(name, out AudioClip audioClip)
            ? audioClip
            : null;

        public void LoadCards()
        {
            CardFaces = Resources.Load<CardFacesStaticData>(CardFacesDataPath);
            CardBacks = Resources.Load<CardBacksStaticData>(CardBacksDataPath);
        }

        public void LoadSettings() =>
            GlobalSettings = Resources.Load<GlobalSettings>(GlobalSettingsDataPath);

        public void LoadVisualSettings() =>
            VisualSettings = Resources.Load<VisualSettings>(VisualSettingsDataPath);

        public void LoadBonusSettings() =>
            BonusSettings = Resources.Load<BonusSettings>(BonusSettingsDataPath);

        public void LoadBonusCards() =>
            BonusCards = Resources.Load<BonusCardsStaticData>(BonusCardsDataPath);

        public LevelData LoadLevel(int levelNumber)
        {
            var txtLvl = Resources.Load<TextAsset>(LevelsDataPath + "/Lvl " + levelNumber);
            return JsonUtility.FromJson<LevelData>(txtLvl.ToString());
        }
    }
}