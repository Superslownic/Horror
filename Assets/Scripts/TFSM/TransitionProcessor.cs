namespace Scripts.TFSM
{
	public abstract class TransitionProcessor
	{
		public bool IsFinished { get; private set; }

		public virtual void Execute() => FinishTransition();

		protected void FinishTransition() => IsFinished = true;
	}
}