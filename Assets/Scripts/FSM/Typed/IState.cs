namespace Scripts.FSM.Typed
{
	public interface IState
	{
		public void OnExit();
	}
	
	public interface IDefaultState : IState
	{
		public void OnEnter();
	}
	
	public interface IPayloadState<in TPayload> : IState
	{
		public void OnEnter(TPayload target);
	}
}