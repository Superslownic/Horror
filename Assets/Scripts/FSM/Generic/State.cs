namespace Scripts.FSM.Generic
{
	public abstract class State
	{
		public virtual string Name { get; set; } = "Unnamed State";
		
		public abstract void Enter();
		public abstract void Update();
		public abstract void Exit();
	}
}