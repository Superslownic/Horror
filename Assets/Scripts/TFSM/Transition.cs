using System;

namespace Scripts.TFSM
{
	public class Transition
	{
		public Type Destination { get; }
		public bool IsValid => _condition.Invoke();
		public bool IsFinished => _processor?.IsFinished ?? true;

		private readonly Func<bool> _condition;
		private readonly TransitionProcessor _processor;

		public Transition(Type destination, Func<bool> condition, TransitionProcessor processor)
		{
			Destination = destination;
			_condition = condition;
			_processor = processor;
		}

		public void Execute() => _processor?.Execute();
	}
}