namespace Scripts.FSM.Composite
{
	public abstract class State
	{
		public string Name { get; }

		public State(string name)
		{
			Name = name;
		}
		
		public abstract void Enter();
		public abstract void Update();
		public abstract void Exit();
	}
}