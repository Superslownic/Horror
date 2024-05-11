using System;
using Scripts.Units;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace Scripts.TFSM
{
	public abstract class StateMachineAbility : Ability
	{
		[ShowInInspector] public StateMachine StateMachine { get; } = new();

		protected abstract Type InitialState { get; }

		private DiContainer _diContainer;

		protected override void OnInitialize()
		{
			base.OnInitialize();
			_diContainer.BindInstance(Unit).AsSingle();
			RegisterStates();
			StateMachine.Enter(InitialState);
		}

		[Inject]
		private void OnInject(DiContainer diContainer)
		{
			_diContainer = new DiContainer(diContainer);
		}

		protected abstract void RegisterStates();

		protected void RegisterState<TState>()
			where TState : State
		{
			TState state = _diContainer.Instantiate<TState>();
			StateMachine.RegisterState(state);
		}

		protected void RegisterTransition<TTransition>()
			where TTransition : Transition
		{
			TTransition transition = _diContainer.Instantiate<TTransition>();
			transition.Initialize();
			StateMachine.RegisterTransition(transition);
		}
	}
}