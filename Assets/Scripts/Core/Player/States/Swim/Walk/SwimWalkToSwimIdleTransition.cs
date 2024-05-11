using Scripts.Input;
using Scripts.TFSM;
using Zenject;

namespace Scripts.Core.Player.States
{
	public class SwimWalkToSwimIdleTransition : Transition<SwimWalkState, SwimIdleState>
	{
		[Inject] private readonly InputManager _inputManager;

		public override bool IsValid => !_inputManager.Move.IsPressed();
	}
}