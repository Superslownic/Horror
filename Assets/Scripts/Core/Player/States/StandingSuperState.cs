using Scripts.FSM.Composite;

namespace Scripts.Core.Player.States
{
	public class StandingSuperState : SuperState
	{
		public StandingSuperState(string name, State initialState) : base(name, initialState)
		{
		}
	}
}