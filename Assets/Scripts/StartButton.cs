using Assets.Arch.Services;
using Assets.Arch.States;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class StartButton : MonoBehaviour
    {
        private StateMachine _stateMachine;

        private void Start()
        {
            _stateMachine = GameServices.Container.Get<StateMachine>();
            GetComponent<Button>().onClick.AddListener(OnStartGameClick);
        }

        private void OnStartGameClick() =>
            _stateMachine.Enter<GameState>();
    }
}
