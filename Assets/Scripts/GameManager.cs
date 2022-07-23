using Assets.Arch.Factories;
using Assets.Arch.Services;
using Assets.Scripts;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private GridLoader _gridLoader;
    private GridCreator _gridCreator;
    private IGameLogic _gameLogic;
    private ISpawnFactory _spawnFactory;
    private int _currentLevel;
    private int _allLevelsLength;

    private void OnDisable() =>
        _gameLogic.LevelCompleted -= OnLevelCompleted;

    public void Construct(
        IGameLogic gameLogic,
        IStaticDataService staticData,
        ISpawnFactory spawnFactory,
        float offset,
        GridLoader gridLoader,
        GridCreator gridCreator)
    {
        _gameLogic = gameLogic;
        _spawnFactory = spawnFactory;
        _gridLoader = gridLoader;
        _gridCreator = gridCreator;

        _gameLogic.Construct(_spawnFactory, staticData.VisualSettings, offset, _gridCreator);
        _gameLogic.LevelCompleted += OnLevelCompleted;
    }

    private void OnLevelCompleted() =>
        LoadNextLevel();

    public void StarLevel(int levelNumber)
    {
        _currentLevel = levelNumber;
        _gridCreator.ClearGrid();
        var gridData = _gridLoader.GetLevelData(levelNumber);
        var ggrid = _gridCreator.Create(gridData, 0.1f, transform);
        var bonusCard = ggrid.Cells.FirstOrDefault(x => x != null && x.Bonus >= 0);
        _gameLogic.StartLevel(ggrid, gridData.CountToMatch, bonusCard);
    }

    public void LoadNextLevel()
    {
        if (_currentLevel + 1 >= _allLevelsLength)
            return;

        _currentLevel++;
        StarLevel(_currentLevel);
    }
}
