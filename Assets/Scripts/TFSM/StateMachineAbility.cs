using System;
using Scripts.Units;
using Sirenix.OdinInspector;
using Zenject;

namespace Scripts.TFSM
{
	public abstract class StateMachineAbility : Ability
	{
		[ShowInInspector] public StateMachine StateMachine { get; } = new();

		protected abstract Type InitialState { get; }

		private DiContainer _diContainer;

		[Inject]
		private void OnInject(DiContainer diContainer)
		{
			_diContainer = new DiContainer(diContainer);
		}

		protected override void OnInitialize()
		{
			base.OnInitialize();
			_diContainer.BindInstance(Unit).AsSingle();
			RegisterStates();
			StateMachine.Enter(InitialState);
		}

		protected abstract void RegisterStates();

		protected void RegisterState<TState>()
			where TState : SuperState
		{
			TState state = _diContainer.Instantiate<TState>();
			state.OnInitialize();
			StateMachine.RegisterState(state);
		}

		protected void RegisterState<TState>(Transition[] transitions)
			where TState : LeafState
		{
			TState state = _diContainer.Instantiate<TState>();
			state.OnInitialize();
			StateMachine.RegisterState(state, transitions);
		}

		protected static Transition To<TState>(Func<bool> when, Action with = null)
			where TState : State
		{
			return new Transition(typeof(TState), when, with != null ? new ActionTransitionProcessor(with) : null);
		}
	}
}