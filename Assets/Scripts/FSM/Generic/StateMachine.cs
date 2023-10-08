using System;
using Scripts.Entities;

namespace Scripts.FSM.Generic
{
	public abstract class StateMachine : Ability
	{
		private CompositeState _compositeState = new();
		
		protected override void OnInitialize()
		{
			State startingState = Setup();
			_compositeState.Enter(startingState);
		}

		protected override void OnUpdate()
		{
			_compositeState.OnUpdate();
		}

		protected abstract State Setup();

		protected void AddTransition(State origin, State destination, Func<bool> condition)
		{
			_compositeState.AddTransition(origin, destination, condition);
		}
	}
}