using Scripts.FSM.Simple;
using Scripts.Input;
using Zenject;

namespace Scripts.Game.States
{
	public class BootstrapState : IDefaultState
	{
		[Inject] private readonly GameStateMachine _stateMachine;
		[Inject] private readonly InputManager _inputManager;

		public void OnEnter()
		{
			_inputManager.Initialize();
		}

		public void OnExit()
		{
		}
	}
}