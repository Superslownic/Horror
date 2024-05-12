using System;

namespace Scripts.TFSM
{
	public class ActionTransitionProcessor : TransitionProcessor
	{
		private Action _action;

		public ActionTransitionProcessor(Action action)
		{
			_action = action;
		}

		public override void Execute()
		{
			_action.Invoke();
			FinishTransition();
		}
	}
}