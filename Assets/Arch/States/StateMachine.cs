using System;
using System.Collections.Generic;
using Assets.Arch.Factories;
using Assets.Arch.Utilities;

namespace Assets.Arch.States
{
    public class StateMachine: IService
    {
        private readonly Dictionary<Type, IState> _states;
        private IState _currentState;

        public StateMachine(SceneLoader sceneLoader, LoadingScreen loadingScreen, ISpawnFactory spawnFactory)
        {
            _states = new Dictionary<Type, IState>()
            {
                [typeof(MenuState)] = new MenuState(this, sceneLoader, loadingScreen),
                [typeof(GameState)] = new GameState(this, sceneLoader, loadingScreen, spawnFactory),
                [typeof(MosaicState)] = new MosaicState(this, sceneLoader, loadingScreen, spawnFactory),
            };
        }

        public void Enter<TState>() where TState : class, IState
        {
            var state = ChangeState<TState>();
            state.Enter();
        }

        private TState ChangeState<TState>() where TState : class, IState
        {
            _currentState?.Exit();
            var state = GetState<TState>();
            _currentState = state;
            return state;
        }

        private TState GetState<TState>() where TState : class, IState =>
            _states[typeof(TState)] as TState;
    }
}