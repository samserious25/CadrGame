using Assets.Arch.Factories;
using Assets.Arch.Utilities;
using Assets.Scripts;
using System;

public interface IGameLogic
{
    void Construct(ISpawnFactory spawnFactory, VisualSettings visualSettings, float offset, GridCreator gridCreator);
    void StartLevel(GGrid<CardData> grid, int countToMatch, CardData bonusCard);
    bool DeleteBonus();
    public event Action<string> PlaySound;
    public event Action LevelCompleted;
    public event Action CorrectMatch;
    public event Action WrongMatch;
    public event Action<int> BonusFound;
    public event Action Tap;
}
