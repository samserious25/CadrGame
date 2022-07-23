using Assets.Arch.StaticData;
using UnityEngine;

public class BonusManager : MonoBehaviour
{
    private IGameLogic _gameLogic;
    private BonusSettings _bonuses;

    // Statistics
    private int _currentCorrectMatchesCount;
    private int _currentWrongMatchesCount;

    private int _totalCorrectMatchesCount;
    private int _totalWrongMatchesCount;
    private int _totalComboCount;

    // For bonus
    private int _comboCount;

    // For bonus card
    private int _totalTaps;

    private void OnDisable()
    {
        _gameLogic.CorrectMatch -= OnCorrectMatch;
        _gameLogic.WrongMatch -= OnWrongMatch;
        _gameLogic.LevelCompleted -= OnLevelCompleted;
        _gameLogic.BonusFound -= OnBonusFound;
        _gameLogic.Tap -= OnTap;
    }

    public void Construct(IGameLogic gameLogic, BonusSettings bonusSettings)
    {
        _gameLogic = gameLogic;
        _bonuses = bonusSettings;

        _gameLogic.CorrectMatch += OnCorrectMatch;
        _gameLogic.WrongMatch += OnWrongMatch;
        _gameLogic.LevelCompleted += OnLevelCompleted;
        _gameLogic.BonusFound += OnBonusFound;
        _gameLogic.Tap += OnTap;
    }

    private void OnCorrectMatch()
    {
        _currentCorrectMatchesCount++;
        _comboCount++;

        CheckForCombo();
        CheckForReduceBonus();
    }

    private void OnWrongMatch()
    {
        _currentWrongMatchesCount++;
        _totalComboCount += _comboCount;
        _comboCount = 0;
    }

    private void OnLevelCompleted()
    {
        _totalCorrectMatchesCount += _currentCorrectMatchesCount;
        _totalWrongMatchesCount += _currentWrongMatchesCount;

        Debug.LogFormat("Correct matches: {0}, Wrong matches: {1}", _currentCorrectMatchesCount, _currentWrongMatchesCount);
        Debug.LogFormat("Overal correct matches: {0}, Overal wrong matches: {1}", _totalCorrectMatchesCount, _totalWrongMatchesCount);

        _currentCorrectMatchesCount = 0;
        _currentWrongMatchesCount = 0;
        _comboCount = 0;
        _totalTaps = 0;
    }

    private void OnBonusFound(int bonus)
    {
        Debug.LogFormat("Bonus {0} found!", bonus);
    }

    private void OnTap()
    {
        _totalTaps++;

        if (_bonuses.TapCountToDeleteBonusCard == 0)
            return;

        if (_totalTaps == _bonuses.TapCountToDeleteBonusCard)
        {
            if (_gameLogic.DeleteBonus())
            {
                _totalTaps = 0;
                Debug.Log("Bonus deleted!");
            }
        }
    }

    private void CheckForCombo()
    {
        if (_comboCount > 1)
        {
            Debug.LogFormat("Combo {0}!", _comboCount);
        }
    }

    private void CheckForReduceBonus()
    {
        if (_comboCount == _bonuses.CorrectMatchesCountForReduceBonus)
        {
            Debug.Log("Earned reduce bonus!");
        }
    }
}
