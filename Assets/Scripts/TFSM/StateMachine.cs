using System;
using System.Collections.Generic;
using System.Linq;
using Scripts.Reactive;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Scripts.TFSM
{
	public class StateMachine
	{
		[ShowInInspector] public State CurrentState { get; private set; }
		
		[ShowInInspector] private Dictionary<Type, State> _states = new();
		[ShowInInspector] private Dictionary<State, List<Transition>> _transitions = new();

		public void AddState(State state, SuperState parent = null, Transition[] transitions = null)
		{
			state.Parent = parent;

			if (transitions != null)
			{
				_transitions.Add(state, new List<Transition>(transitions));
			}

			_states.Add(state.GetType(), state);
		}

		public void Update()
		{
			CurrentState.OnUpdate();

			if (FindPossibleTransition(out Type nextStateType))
			{
				Enter(nextStateType);
			}
		}

		public void Enter(Type targetStateType)
		{
			if (!_states.ContainsKey(targetStateType))
			{
				Debug.LogError("targetState is not found");
				return;
			}

			State targetState = _states[targetStateType];

			if (targetState is not SubState)
			{
				Debug.LogError("targetState is not SubState");
				return;
			}

			if (CurrentState == targetState)
			{
				return;
			}

			State firstSameParent = GetFirstSameParent(CurrentState, targetState);
			
			if (CurrentState != null)
			{
				Exit(CurrentState, firstSameParent);
			}

			Enter(targetState, firstSameParent);

			CurrentState = targetState;
		}

		public void Enter<T>() where T : SubState
		{
			Enter(typeof(T));
		}

		private void Exit(State state, State until)
		{
			if(state.Parent != null && state.Parent != until)
			{
				Exit(state.Parent, until);
			}

			state.OnExit();
		}
		
		private void Enter(State state, State until)
		{
			if(state.Parent != null && state.Parent != until)
			{
				Enter(state.Parent, until);
			}
			
			state.OnEnter();
		}

		private State GetFirstSameParent(State firstState, State secondState)
		{
			if (firstState == null || secondState == null)
			{
				return null;
			}
			
			return GetSameParents(firstState, secondState).FirstOrDefault();
		}

		private IEnumerable<State> GetSameParents(State firstState, State secondState)
		{
			IEnumerable<State> firstStateParents = GetParents(firstState);
			IEnumerable<State> secondStateParents = GetParents(secondState);
			return firstStateParents.Intersect(secondStateParents);
		}

		private IEnumerable<State> GetParents(State state)
		{
			while (state.Parent != null)
			{
				yield return state.Parent;
				state = state.Parent;
			}
		}
		
		private bool FindPossibleTransition(out Type result)
		{
			if (CurrentState == null || !_transitions.ContainsKey(CurrentState))
			{
				result = default;
				return false;
			}

			List<Transition> transitions = _transitions[CurrentState];

			for (int i = 0; i < transitions.Count; i++)
			{
				Transition transition = transitions[i];
				
				if (transition.IsValid)
				{
					result = transition.Destination;
					return true;
				}
			}
			
			result = default;
			return false;
		}
	}

	public abstract class State
	{
		public State Parent { get; set; }

		public virtual void OnEnter() { }
		
		public virtual void OnUpdate() { }
		
		public virtual void OnExit() { }
	}

	public abstract class SuperState : State
	{
	}
	
	public abstract class SubState : State
	{
	}

	public abstract class Transition
	{
		public abstract Type Destination { get; }
		public abstract bool IsValid { get; }
		
		public static Transition To<T>(Func<bool> when) where T : SubState
		{
			return new ConditionTransitionTo<T>(when);
		}
		
		public static Transition To<T>(DisposableAction when) where T : SubState
		{
			return new ObservableTransitionTo<T>(when);
		}
	}

	public abstract class TypedTransition<T> : Transition where T : SubState
	{
		public override Type Destination { get; } = typeof(T);
	}

	public class ConditionTransitionTo<T> : TypedTransition<T> where T : SubState
	{
		public ConditionTransitionTo(Func<bool> when)
		{
			_condition = when;
		}
		
		public override bool IsValid => _condition.Invoke();
		
		private Func<bool> _condition;
	}
	
	public class ObservableTransitionTo<T> : TypedTransition<T> where T : SubState
	{
		public ObservableTransitionTo(IObservable when)
		{
			when.AddListener(() => _wasInvoked = true);
		}

		public override bool IsValid
		{
			get
			{
				if (_wasInvoked)
				{
					_wasInvoked = false;
					return true;
				}

				return false;
			}
		}

		private bool _wasInvoked;
	}
	
	public class Standing : SuperState
	{
	}

	public class StandingIdle : SubState
	{
	}
	
	public class StandingWalk : SubState
	{
	}
	
	public class StandingRun : SubState
	{
	}
	
	public class Crouching : SuperState
	{
	}

	public class CrouchingIdle : SubState
	{
	}
	
	public class CrouchingWalk : SubState
	{
	}

	public class LadderClimbing : SuperState
	{
	}

	public class LadderClimbingIdle : SubState
	{
	}
	
	public class LadderClimbingWalk : SubState
	{
	}
	
	public class LadderClimbingRun : SubState
	{
	}
}