using System;
using System.Collections.Generic;

namespace Scripts.FSM.Simple
{
	public class StateMachine
	{
		private readonly Dictionary<Type, IState> _states = new();

		private IState _currentState;

		public void AddState(IState state)
		{
			_states.Add(state.GetType(), state);
		}

		public void Enter<TState>(TState state) where TState : IDefaultState
		{
			if(_currentState is TState)
				return;
			
			IState lastState = _currentState;
			TState nextState = GetState<TState>();
			_currentState = nextState;

			lastState?.OnExit();
			nextState.OnEnter();
		}

		public void Enter<TState>() where TState : IDefaultState
		{
			if(_currentState is TState)
				return;
			
			IState lastState = _currentState;
			TState nextState = GetState<TState>();
			_currentState = nextState;

			lastState?.OnExit();
			nextState.OnEnter();
		}
		
		public void Enter<TState, TPayload>(TPayload payload) where TState : IPayloadState<TPayload>
		{
			if(_currentState is TState)
				return;
			
			IState lastState = _currentState;
			TState nextState = GetState<TState>();
			_currentState = nextState;

			lastState?.OnExit();
			nextState.OnEnter(payload);
		}

		private TState GetState<TState>() where TState : IState
		{
			return (TState) _states[typeof(TState)];
		}
	}
}
