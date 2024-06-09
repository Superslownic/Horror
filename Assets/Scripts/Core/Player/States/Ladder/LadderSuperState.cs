using Scripts.TFSM;
using Scripts.Units;
using Zenject;

namespace Scripts.Core.Player.States
{
	public class LadderSuperState : SuperState
	{
		[Inject] private readonly Unit _playerUnit;

		public override void OnEnter()
		{
			_playerUnit.GetAbility<LadderClimbAbility>().Mount();
			_playerUnit.GetAbility<RigidbodyAbility>().Rigidbody.linearDamping = 5;
		}

		public override void OnExit()
		{
			_playerUnit.GetAbility<RigidbodyAbility>().Rigidbody.linearDamping = 0;
		}
	}
}