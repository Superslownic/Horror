using System;

namespace Scripts.TFSM
{
	public abstract class LeafState : State
	{
		public override Type Parent => null;
	}

	public abstract class LeafState<TParent> : LeafState
		where TParent : SuperState
	{
		public override Type Parent { get; } = typeof(TParent);
	}
}