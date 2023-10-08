using System;

namespace Scripts.FSM.Generic
{
	public sealed class Transition
	{
		public readonly State Destination;
		public readonly Func<bool> Condition;

		public Transition(State destination, Func<bool> condition)
		{
			Destination = destination;
			Condition = condition;
		}
	}
}