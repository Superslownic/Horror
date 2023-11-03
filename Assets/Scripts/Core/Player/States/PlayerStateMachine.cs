using Scripts.Factory;
using Scripts.FSM.Composite;
using Scripts.Input;
using Zenject;

namespace Scripts.Core.Player.States
{
	public class PlayerStateMachine : StateMachineAbility
	{
		[Inject] private readonly ObjectFactory _objectFactory;
		[Inject] private readonly InputManager _inputManager;
		
		protected override void Setup()
		{
			State standing = SetupStandingSuperState();
			State crouching = SetupCrouchingSuperState();

			Root = new SuperState("Root", standing);
			
			Root.AddState(standing, transitions: new Transition[]
			{
				new (crouching, when: () => _inputManager.Crouch.WasPressedThisFrame())
			});
			
			Root.AddState(crouching, transitions: new Transition[]
			{
				new (standing, when: () => _inputManager.Crouch.WasPressedThisFrame() || _inputManager.Run.WasPressedThisFrame())
			});
		}

		private State SetupStandingSuperState()
		{
			State idle = _objectFactory.CreateInjectedInstance<StandingIdleState>("Idle", Unit);
			State walk = _objectFactory.CreateInjectedInstance<StandingWalkState>("Walk", Unit);
			State run = _objectFactory.CreateInjectedInstance<StandingRunState>("Run", Unit);
			
			SuperState standing = new(name: "Standing", initialState: idle);
			
			standing.AddState(idle, transitions: new Transition[]
			{
				new (destination: run, when: () => _inputManager.Move.IsPressed() && _inputManager.Run.WasPressedThisFrame()),
				new (destination: walk, when: () => _inputManager.Move.IsPressed())
			});
			
			standing.AddState(walk, transitions: new Transition[]
			{
				new (destination: idle, when: () => !_inputManager.Move.IsPressed()),
				new (destination: run, when: () => _inputManager.Run.WasPressedThisFrame())
			});
			
			standing.AddState(run, transitions: new Transition[]
			{
				new (destination: idle, when: () => !_inputManager.Move.IsPressed()),
				new (destination: walk, when: () => _inputManager.Run.WasPressedThisFrame())
			});

			return standing;
		}
		
		private State SetupCrouchingSuperState()
		{
			State idle = _objectFactory.CreateInjectedInstance<CrouchingIdleState>("Idle", Unit);
			State walk = _objectFactory.CreateInjectedInstance<CrouchingWalkState>("Walk", Unit);
			
			SuperState crouching = new(name: "Crouching", initialState: idle);
			
			crouching.AddState(idle, transitions: new Transition[]
			{
				new (destination: walk, when: () => _inputManager.Move.IsPressed())
			});
			
			crouching.AddState(state: walk, transitions: new Transition[]
			{
				new (destination: idle, when: () => !_inputManager.Move.IsPressed())
			});

			return crouching;
		}
	}
}