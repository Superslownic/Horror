using System;
using Scripts.Entities;

namespace Scripts.FSM.Generic
{
	public abstract class StateMachineAbility : Ability
	{
		public StateMachine StateMachine { get; } = new();
		
		protected override void OnInitialize()
		{
			Setup();
		}

		protected override void OnActivate()
		{
			StateMachine.Enter();
		}

		protected override void OnUpdate()
		{
			StateMachine.Update();
		}

		protected override void OnDeactivate()
		{
			StateMachine.Exit();
		}

		protected abstract void Setup();

		protected void SetStartState(State state)
		{
			StateMachine.SetStartState(state);
		}

		protected void AddTransition(State origin, State destination, Func<bool> condition)
		{
			StateMachine.AddTransition(origin, destination, condition);
		}
	}
}