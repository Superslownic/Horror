using Scripts.FSM.Composite;
using Scripts.Units;
using Zenject;

namespace Scripts.Core.Player.States
{
	public class SwimToStandState : State
	{
		[Inject] private readonly Unit _playerUnit;

		public SwimToStandState(string name) : base(name) { }

		public override void Enter()
		{
			_playerUnit.GetAbility<ChangeHeightAbility>().ResizeToFitSurface();
		}

		public override void Update() { }

		public override void Exit() { }
	}
}