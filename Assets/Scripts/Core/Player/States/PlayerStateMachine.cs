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
			State swimming = SetupSwimmingSuperState();
			State swimToStand = _objectFactory.CreateInjectedInstance<SwimToStandState>("SwimToStand", Unit);

			Root = new SuperState("Root", initialState: standing);

			Root.AddState(state: standing, transitions: new[]
			{
				new Transition(destination: crouching, when: () => _inputManager.Crouch.WasPressedThisFrame()),
				new Transition(destination: swimming, when: () => Unit.GetAbility<PlayerCheckWaterAbility>().InWater)
			});

			Root.AddState(state: crouching, transitions: new[]
			{
				new Transition(destination: standing, when: () => _inputManager.Crouch.WasPressedThisFrame() || _inputManager.Run.WasPressedThisFrame()),
				new Transition(destination: swimming, when: () => Unit.GetAbility<PlayerCheckWaterAbility>().InWater)
			});

			Root.AddState(state: swimming, transitions: new[]
			{
				new Transition(destination: swimToStand, when: () => !Unit.GetAbility<PlayerCheckWaterAbility>().InWater)
			});

			Root.AddState(state: swimToStand, transitions: new[]
			{
				new Transition(destination: standing, when: () => true)
			});
		}

		private State SetupStandingSuperState()
		{
			State idle = _objectFactory.CreateInjectedInstance<StandingIdleState>("Idle", Unit);
			State walk = _objectFactory.CreateInjectedInstance<StandingWalkState>("Walk", Unit);
			State run = _objectFactory.CreateInjectedInstance<StandingRunState>("Run", Unit);
			
			StandingSuperState standing = _objectFactory.CreateInjectedInstance<StandingSuperState>("Standing", Unit, idle);
			
			standing.AddState(state: idle, transitions: new Transition[]
			{
				new (destination: run, when: () => _inputManager.Move.IsPressed() && _inputManager.Run.WasPressedThisFrame()),
				new (destination: walk, when: () => _inputManager.Move.IsPressed())
			});
			
			standing.AddState(state: walk, transitions: new Transition[]
			{
				new (destination: idle, when: () => !_inputManager.Move.IsPressed()),
				new (destination: run, when: () => _inputManager.Run.WasPressedThisFrame())
			});
			
			standing.AddState(state: run, transitions: new Transition[]
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

			CrouchingSuperState crouching = _objectFactory.CreateInjectedInstance<CrouchingSuperState>("Crouching", Unit, idle);

			crouching.AddState(state: idle, transitions: new Transition[]
			{
				new (destination: walk, when: () => _inputManager.Move.IsPressed())
			});

			crouching.AddState(state: walk, transitions: new Transition[]
			{
				new (destination: idle, when: () => !_inputManager.Move.IsPressed())
			});

			return crouching;
		}

		private State SetupSwimmingSuperState()
		{
			State idle = _objectFactory.CreateInjectedInstance<SwimmingIdleState>("Idle", Unit);
			State move = _objectFactory.CreateInjectedInstance<SwimmingMoveState>("Move", Unit);

			SwimmingSuperState swimming = _objectFactory.CreateInjectedInstance<SwimmingSuperState>("Swimming", Unit, idle);

			swimming.AddState(state: idle, transitions: new Transition[]
			{
				new (destination: move, when: () => _inputManager.Move.IsPressed())
			});

			swimming.AddState(state: move, transitions: new Transition[]
			{
				new (destination: idle, when: () => !_inputManager.Move.IsPressed())
			});

			return swimming;
		}
	}
}