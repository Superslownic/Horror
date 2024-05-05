using Scripts.FSM.Composite;
using Scripts.Units;
using Zenject;

namespace Scripts.Core.Player.States
{
	public class StandingSuperState : SuperState
	{
		[Inject] private readonly Unit _playerUnit;

		public StandingSuperState(string name, State initialState) : base(name, initialState)
		{
		}

		public override void Enter()
		{
			base.Enter();

			if (_playerUnit.TryGetAbility(out HeadBobAbility headBobAbility))
			{
				headBobAbility.AddActivator(this);
			}
		}

		public override void Exit()
		{
			base.Exit();

			if (_playerUnit.TryGetAbility(out HeadBobAbility headBobAbility))
			{
				headBobAbility.RemoveActivator(this);
			}
		}
	}
}