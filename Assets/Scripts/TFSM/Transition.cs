using System;
using Zenject;

namespace Scripts.TFSM
{
	public abstract class Transition : IInitializable
	{
		public abstract Type From { get; }
		public abstract Type To { get; }
		public abstract bool IsValid { get; }

		public bool IsFinished { get; private set; }

		public virtual void Initialize() { }
		public virtual void Execute() => FinishTransition();

		protected void FinishTransition() => IsFinished = true;
	}

	public abstract class Transition<TFrom, TTo> : Transition
		where TFrom : LeafState
		where TTo : LeafState
	{
		public sealed override Type From { get; } = typeof(TFrom);
		public sealed override Type To { get; } = typeof(TTo);
	}
}