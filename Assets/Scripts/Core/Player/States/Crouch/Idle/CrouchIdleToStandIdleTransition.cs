using Scripts.Input;
using Scripts.TFSM;
using Zenject;

namespace Scripts.Core.Player.States
{
	public class CrouchIdleToStandIdleTransition : Transition<CrouchIdleState, StandIdleState>
	{
		[Inject] private readonly InputManager _inputManager;

		public override bool IsValid => _inputManager.Crouch.WasPressedThisFrame() || _inputManager.Jump.WasPressedThisFrame();
	}
}