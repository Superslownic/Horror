using Scripts.Input;
using Scripts.TFSM;
using Zenject;

namespace Scripts.Core.Player.States
{
	public class StandWalkToStandIdleTransition : Transition<StandWalkState, StandIdleState>
	{
		[Inject] private readonly InputManager _inputManager;

		public override bool IsValid => !_inputManager.Move.IsPressed();
	}
}