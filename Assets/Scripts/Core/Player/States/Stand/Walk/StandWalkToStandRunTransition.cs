using Scripts.Input;
using Scripts.TFSM;
using Zenject;

namespace Scripts.Core.Player.States
{
	public class StandWalkToStandRunTransition : Transition<StandWalkState, StandRunState>
	{
		[Inject] private readonly InputManager _inputManager;

		public override bool IsValid => _inputManager.Run.WasPressedThisFrame();
	}
}