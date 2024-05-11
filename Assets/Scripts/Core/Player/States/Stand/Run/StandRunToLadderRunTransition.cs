using Scripts.Reactive;
using Scripts.TFSM;
using Scripts.Units;
using Zenject;

namespace Scripts.Core.Player.States
{
	public class StandRunToLadderRunTransition : Transition<StandRunState, LadderRunState>
	{
		public override bool IsValid => _observableTrigger.IsChecked;

		[Inject] private readonly Unit _playerUnit;

		private ObservableTrigger _observableTrigger;

		public override void Initialize()
		{
			_observableTrigger = new ObservableTrigger(_playerUnit.GetAbility<LadderClimbAbility>().LadderDetectedAction);
		}
	}
}