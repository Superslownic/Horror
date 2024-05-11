using System;

namespace Scripts.TFSM
{
	public abstract class SuperState : State
	{
		public override Type Parent => null;
	}

	public abstract class SuperState<TParent> : SuperState
	{
		public override Type Parent { get; } = typeof(TParent);
	}
}