using System;

namespace Scripts.FSM.Composite
{
	public sealed class Transition
	{
		public readonly State Destination;
		public readonly Func<bool> When;

		public Transition(State destination, Func<bool> when)
		{
			Destination = destination;
			When = when;
		}
	}
}