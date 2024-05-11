using Scripts.Factory;
using Scripts.FSM.Composite;
using Scripts.Input;
using Zenject;

namespace Scripts.Core.Player.States
{
	public class PlayerStateMachine : StateMachineAbility
	{
		protected override void Setup() { }

		/*[Inject] private readonly ObjectFactory _objectFactory;
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
				new Transition(destination: swimming, when: () => Unit.GetAbility<PlayerCheckWaterAbility>().IsInWater && !Unit.GetAbility<PlayerCheckWaterAbility>().CanStand)
			});

			Root.AddState(state: crouching, transitions: new[]
			{
				new Transition(destination: standing, when: () => _inputManager.Crouch.WasPressedThisFrame() || _inputManager.Run.WasPressedThisFrame()),
				new Transition(destination: swimming, when: () => Unit.GetAbility<PlayerCheckWaterAbility>().IsInWater && !Unit.GetAbility<PlayerCheckWaterAbility>().CanStand)
			});

			Root.AddState(state: swimming, transitions: new[]
			{
				new Transition(destination: swimToStand, when: () => Unit.GetAbility<PlayerCheckWaterAbility>().CanStand)
			});

			Root.AddState(state: swimToStand, transitions: new[]
			{
				new Transition(destination: standing, when: () => true)
			});
		}

		private State SetupStandingSuperState()
		{
			State idle = _objectFactory.CreateInjectedInstance<StandIdleState>("Idle", Unit);
			State walk = _objectFactory.CreateInjectedInstance<StandWalkState>("Walk", Unit);
			State run = _objectFactory.CreateInjectedInstance<StandRunState>("Run", Unit);
			
			StandSuperState stand = _objectFactory.CreateInjectedInstance<StandSuperState>("Standing", Unit, idle);
			
			stand.AddState(state: idle, transitions: new Transition[]
			{
				new (destination: run, when: () => _inputManager.Move.IsPressed() && _inputManager.Run.WasPressedThisFrame()),
				new (destination: walk, when: () => _inputManager.Move.IsPressed())
			});
			
			stand.AddState(state: walk, transitions: new Transition[]
			{
				new (destination: idle, when: () => !_inputManager.Move.IsPressed()),
				new (destination: run, when: () => _inputManager.Run.WasPressedThisFrame())
			});
			
			stand.AddState(state: run, transitions: new Transition[]
			{
				new (destination: idle, when: () => !_inputManager.Move.IsPressed()),
				new (destination: walk, when: () => _inputManager.Run.WasPressedThisFrame())
			});

			return stand;
		}
		
		private State SetupCrouchingSuperState()
		{
			State idle = _objectFactory.CreateInjectedInstance<CrouchIdleState>("Idle", Unit);
			State walk = _objectFactory.CreateInjectedInstance<CrouchWalkState>("Walk", Unit);

			CrouchSuperState crouch = _objectFactory.CreateInjectedInstance<CrouchSuperState>("Crouching", Unit, idle);

			crouch.AddState(state: idle, transitions: new Transition[]
			{
				new (destination: walk, when: () => _inputManager.Move.IsPressed())
			});

			crouch.AddState(state: walk, transitions: new Transition[]
			{
				new (destination: idle, when: () => !_inputManager.Move.IsPressed())
			});

			return crouch;
		}

		private State SetupSwimmingSuperState()
		{
			State idle = _objectFactory.CreateInjectedInstance<SwimIdleState>("Idle", Unit);
			State move = _objectFactory.CreateInjectedInstance<SwimWalkState>("Move", Unit);

			SwimSuperState swim = _objectFactory.CreateInjectedInstance<SwimSuperState>("Swimming", Unit, idle);

			swim.AddState(state: idle, transitions: new Transition[]
			{
				new (destination: move, when: () => _inputManager.Move.IsPressed())
			});

			swim.AddState(state: move, transitions: new Transition[]
			{
				new (destination: idle, when: () => !_inputManager.Move.IsPressed())
			});

			return swim;
		}*/
	}
}