using System;
using System.Collections.Generic;

namespace Scripts.FSM.Generic
{
	public class CompositeState : State
	{
		private readonly Dictionary<State, IList<Transition>> _transitions = new();

		private State _currentState;

		public override void OnEnter()
		{
			_currentState?.OnEnter();
		}

		public override void OnUpdate()
		{
			_currentState?.OnUpdate();
			
			if (FindValidTransition(out Transition transition))
				Enter(transition.Destination);
		}

		public override void OnExit()
		{
			_currentState?.OnExit();
		}

		public void Enter(State state)
		{
			if (_currentState != null && _currentState == state)
				return;

			_currentState?.OnExit();
			_currentState = state;
			_currentState?.OnEnter();
		}

		public void AddTransition(State origin, State destination, Func<bool> condition)
		{
			if (!_transitions.ContainsKey(origin))
				_transitions.Add(origin, new List<Transition>());

			_transitions[origin].Add(new Transition(destination, condition));
		}

		private bool FindValidTransition(out Transition result)
		{
			result = default;
			
			if (!_transitions.ContainsKey(_currentState))
			{
				return false;
			}
			
			foreach (Transition transition in _transitions[_currentState])
			{
				if (transition.Condition.Invoke())
				{
					result = transition;
					return true;
				}
			}
			
			return false;
		}
	}
}