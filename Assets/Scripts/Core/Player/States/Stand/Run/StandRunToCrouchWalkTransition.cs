using Scripts.Input;
using Scripts.TFSM;
using Zenject;

namespace Scripts.Core.Player.States
{
	public class StandRunToCrouchWalkTransition : Transition<StandRunState, CrouchWalkState>
	{
		[Inject] private readonly InputManager _inputManager;

		public override bool IsValid => _inputManager.Crouch.WasPressedThisFrame();
	}
}