using System.Collections.Generic;
using Scripts.Reactive;

namespace Scripts.FSM.Composite
{
	public class SuperState : State
	{
		public DisposableAction StateChanged { get; } = new();
		public State CurrentState { get; private set; }
		
		private readonly Dictionary<int, State> _states = new();
		private readonly Dictionary<State, IList<Transition>> _transitions = new();
		private readonly State _initialState;
		private readonly CompositeDisposable _disposable = new();
		
		public SuperState(string name, State initialState) : base(name)
		{
			_initialState = initialState;
		}

		public override void Enter()
		{
			Enter(_initialState);
			ChangeStateIfPossible();
		}

		public override void Update()
		{
			CurrentState?.Update();
			ChangeStateIfPossible();
		}

		public override void Exit()
		{
			CurrentState?.Exit();
			CurrentState = null;
		}

		public void AddState(State state, params Transition[] transitions)
		{
			_states.Add(state.GetHashCode(), state);
			
			foreach (Transition transition in transitions)
			{
				if (!_transitions.ContainsKey(state))
					_transitions.Add(state, new List<Transition>());
				
				_transitions[state].Add(new Transition(transition.Destination, transition.When));
			}

			if (state is SuperState stateMachine)
			{
				stateMachine.StateChanged.AddListener(StateChanged.Invoke).AddTo(_disposable);
			}
		}

		private void Enter(State state)
		{
			if (CurrentState != null && CurrentState == state)
				return;

			CurrentState?.Exit();
			CurrentState = state;
			CurrentState?.Enter();
			StateChanged.Invoke();
		}

		private void ChangeStateIfPossible()
		{
			if (FindValidTransition(out State state))
			{
				Enter(state);
			}
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
				if (transition.When.Invoke())
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