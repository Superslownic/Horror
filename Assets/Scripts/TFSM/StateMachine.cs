using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace Scripts.TFSM
{
	public class StateMachine
	{
		[ShowInInspector] public string CurrentStateName => CurrentState?.GetType().GetNiceName() ?? "Null";

		public State CurrentState { get; private set; }

		private Dictionary<Type, State> _states = new();
		private Dictionary<Type, List<Transition>> _transitions = new();
		private Transition _currentTransition;

		public void RegisterState(SuperState state)
		{
			_states.Add(state.GetType(), state);
		}

		public void RegisterState(LeafState state, IEnumerable<Transition> transitions)
		{
			Type type = state.GetType();

			_states.Add(type, state);

			if(transitions == null)
				return;

			if(!_transitions.ContainsKey(type))
				_transitions.Add(type, new List<Transition>());

			_transitions[type].AddRange(transitions);
		}

		public void Update()
		{
			if(_currentTransition == null)
			{
				CurrentState.OnUpdate();

				if (FindPossibleTransition(out Transition transition))
					_currentTransition = transition;
			}

			if (_currentTransition == null)
				return;

			_currentTransition.Execute();

			if (!_currentTransition.IsFinished)
				return;

			Enter(_currentTransition.Destination);
			_currentTransition = null;
		}

		public void Enter(Type targetStateType)
		{
			if (!_states.TryGetValue(targetStateType, out State targetState))
			{
				Debug.LogError("targetState is not found");
				return;
			}

			if (targetState is not LeafState)
			{
				Debug.LogError("targetState is not SubState");
				return;
			}

			if (CurrentState == targetState)
			{
				return;
			}

			_currentTransition = null;

			Type currentStateType = CurrentState?.GetType();
			Type firstSameParent = GetFirstSameParent(currentStateType, targetStateType);

			if (CurrentState != null)
			{
				Exit(currentStateType, until: firstSameParent);
			}

			Enter(targetStateType, until: firstSameParent);

			CurrentState = targetState;
		}

		public void Enter<T>() where T : LeafState
		{
			Enter(typeof(T));
		}

		private void Exit(Type from, Type until)
		{
			State state = _states[from];

			if(state.Parent != null && state.Parent != until)
			{
				Exit(state.Parent, until);
			}

			state.OnExit();
		}

		private void Enter(Type from, Type until)
		{
			State state = _states[from];

			if(state.Parent != null && state.Parent != until)
			{
				Enter(state.Parent, until);
			}

			state.OnEnter();
		}

		private Type GetFirstSameParent(Type firstState, Type secondState)
		{
			if (firstState == null || secondState == null)
			{
				return null;
			}

			return GetSameParents(firstState, secondState).FirstOrDefault();
		}

		private IEnumerable<Type> GetSameParents(Type firstState, Type secondState)
		{
			IEnumerable<Type> firstStateParents = GetParents(firstState);
			IEnumerable<Type> secondStateParents = GetParents(secondState);
			return firstStateParents.Intersect(secondStateParents);
		}

		private IEnumerable<Type> GetParents(Type from)
		{
			State state = _states[from];

			while (state.Parent != null)
			{
				yield return state.Parent;
				state = _states[state.Parent];
			}
		}

		private bool FindPossibleTransition(out Transition result)
		{
			if(CurrentState == null)
			{
				result = default;
				return false;
			}

			Type type = CurrentState.GetType();

			if (!_transitions.TryGetValue(type, out List<Transition> transitions))
			{
				result = default;
				return false;
			}

			for (int i = 0; i < transitions.Count; i++)
			{
				Transition transition = transitions[i];

				if (transition.IsValid)
				{
					result = transition;
					return true;
				}
			}

			result = default;
			return false;
		}
	}
}