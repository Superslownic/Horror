using Scripts.TFSM;
using Scripts.Units;
using Zenject;

namespace Scripts.Core.Player.States
{
	public class LadderSuperState : SuperState
	{
		[Inject] private readonly Unit _playerUnit;

		private LadderClimbAbility _ladderClimbAbility;

		public override void OnInitialize()
		{
			_ladderClimbAbility = _playerUnit.GetAbility<LadderClimbAbility>();
		}

		public override void OnEnter()
		{
			_ladderClimbAbility.Mount();
		}
	}
}