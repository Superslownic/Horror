using System;

namespace Scripts.TFSM
{
	public abstract class State
	{
		public abstract Type Parent { get; }

		public virtual void OnEnter() { }
		
		public virtual void OnUpdate() { }
		
		public virtual void OnExit() { }
	}
}