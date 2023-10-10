using Scripts.Factory;
using Scripts.FSM.Simple;
using Zenject;

namespace Scripts.Game.States
{
	public class GameStateMachine : StateMachine
	{
		[Inject] private readonly ObjectFactory _objectCreator;

		[Inject]
		private void Construct()
		{
			AddState(_objectCreator.CreateInjectedInstance<BootstrapState>());
			Enter<BootstrapState>();
		}
	}
}