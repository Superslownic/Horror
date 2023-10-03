using Scripts.Factory;
using Zenject;

namespace Scripts.FSM
{
	public class GameStateMachine : StateMachine
	{
		[Inject] private readonly IObjectFactory _objectCreator;

		[Inject]
		private void Construct()
		{
			AddState(_objectCreator.CreateInjectedInstance<BootstrapState>());
			Enter<BootstrapState>();
		}
	}
}