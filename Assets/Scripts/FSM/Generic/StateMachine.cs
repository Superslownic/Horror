using System;
using System.Collections.Generic;

namespace Scripts.FSM.Generic
{
	public class StateMachine : State
	{
		public State CurrentState { get; private set; }
		
		private readonly Dictionary<State, IList<Transition>> _transitions = new();

		private State _startState;

		public override void Enter()
		{
			Enter(_startState);
		}

		public override void Update()
		{
			CurrentState?.Update();
			
			if (FindValidTransition(out State state))
			{
				Enter(state);
			}
		}

		public override void Exit()
		{
			CurrentState?.Exit();
			CurrentState = null;
		}

		public void SetStartState(State state)
		{
			_startState = state;
		}

		public void AddTransition(State origin, State destination, Func<bool> condition)
		{
			if (!_transitions.ContainsKey(origin))
				_transitions.Add(origin, new List<Transition>());

			_transitions[origin].Add(new Transition(destination, condition));
		}

		private void Enter(State state)
		{
			if (CurrentState != null && CurrentState == state)
				return;

			CurrentState?.Exit();
			CurrentState = state;
			CurrentState?.Enter();
		}

		private bool FindValidTransition(out State result)
		{
			if (CurrentState == null || !_transitions.ContainsKey(CurrentState))
			{
				result = default;
				return false;
			}
			
			foreach (Transition transition in _transitions[CurrentState])
			{
				if (transition.Condition.Invoke())
				{
					result = transition.Destination;
					return true;
				}
			}
			
			result = default;
			return false;
		}
	}
}