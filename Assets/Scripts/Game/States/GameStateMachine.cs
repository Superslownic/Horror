using Scripts.Factory;
using Scripts.FSM.Typed;
using Zenject;

namespace Scripts.Game.States
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