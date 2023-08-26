using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Puhinsky.StateMachine
{
    public class StateMachine<TInitializer>
    {
        private IState<TInitializer> _currentState;
        private readonly Dictionary<Type, IState<TInitializer>> _states;
        private bool _isUpdate;

        public StateMachine(params IState<TInitializer>[] states)
        {
            _states = new Dictionary<Type, IState<TInitializer>>(states.Length);

            foreach (var state in states)
            {
                _states.Add(state.GetType(), state);
            }
        }

        public void SwitchState<TState>() where TState : IState<TInitializer>
        {
            TryExit();
            SetNewState<TState>();
            TryEnter();
            TryUpdate();
        }

        private void TryEnter()
        {
            if (_currentState is IEnterable enterable)
                enterable.OnEnter();
        }

        private void TryUpdate()
        {
            if (_currentState is IUpdatable updatable)
            {
                _isUpdate = true;
                Update(updatable);
            }
            else
            {
                _isUpdate = false;
            }
        }

        private void TryExit()
        {
            if (_currentState is IExitable exitable)
                exitable.OnExit();
        }

        private void SetNewState<TState>() where TState : IState<TInitializer>
        {
            _currentState = GetState<TState>();
        }

        private TState GetState<TState>() where TState : IState<TInitializer>
        {
            return (TState)_states[typeof(TState)];
        }

        private async void Update(IUpdatable updatable)
        {
            while (_isUpdate)
            {
                updatable.OnUpdate();
                await Task.Yield();
            }
        }
    }
}
