using Scripts.Input;
using Scripts.TFSM;
using Zenject;

namespace Scripts.Core.Player.States
{
	public class SwimIdleToSwimWalkTransition : Transition<SwimIdleState, SwimWalkState>
	{
		[Inject] private readonly InputManager _inputManager;

		public override bool IsValid => _inputManager.Move.IsPressed();
	}
}