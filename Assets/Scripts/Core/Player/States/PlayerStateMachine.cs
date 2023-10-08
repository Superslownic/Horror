using Scripts.Factory;
using Scripts.FSM.Generic;
using Zenject;

namespace Scripts.Core.Player.States
{
	public class PlayerStateMachine : StateMachineAbility
	{
		[Inject] private readonly IObjectFactory _objectFactory;
		
		protected override void Setup()
		{
			State standing = SetupStandingState();
			State crouching = SetupCrouchingState();
			
			AddTransition(standing, crouching, () => false);
			AddTransition(crouching, standing, () => false);
			
			SetStartState(standing);
		}

		private State SetupStandingState()
		{
			StateMachine standing = new();
			
			State idle = _objectFactory.CreateInjectedInstance<PlayerStandingIdleState>();
			State walk = _objectFactory.CreateInjectedInstance<PlayerStandingWalkState>();
			State run = _objectFactory.CreateInjectedInstance<PlayerStandingRunState>();
			
			standing.AddTransition(idle, walk, () => false);
			standing.AddTransition(idle, run, () => false);
			standing.AddTransition(walk, idle, () => false);
			standing.AddTransition(walk, run, () => false);
			
			standing.SetStartState(idle);

			standing.Name = "Standing";

			return standing;
		}
		
		private State SetupCrouchingState()
		{
			StateMachine crouching = new();
			
			State idle = _objectFactory.CreateInjectedInstance<PlayerCrouchingIdleState>();
			State walk = _objectFactory.CreateInjectedInstance<PlayerCrouchingWalkState>();
			
			crouching.AddTransition(idle, walk, () => false);
			crouching.AddTransition(walk, idle, () => false);
			
			crouching.SetStartState(idle);
			
			crouching.Name = "Crouching";

			return crouching;
		}
	}
	
	public class PlayerStandingIdleState : State
	{
		public override string Name { get; set; } = "Idle";

		public override void Enter()
		{
			
		}

		public override void Update()
		{
		}

		public override void Exit()
		{
		}
	}

	public class PlayerStandingWalkState : State
	{
		public override string Name { get; set; } = "Walk";
		
		public override void Enter()
		{
			
		}

		public override void Update()
		{
		}

		public override void Exit()
		{
		}
	}
	
	public class PlayerStandingRunState : State
	{
		public override string Name { get; set; } = "Run";
		
		public override void Enter()
		{
			
		}

		public override void Update()
		{
		}

		public override void Exit()
		{
		}
	}
	
	public class PlayerCrouchingIdleState : State
	{
		public override string Name { get; set; } = "Idle";
		
		public override void Enter()
		{
			
		}

		public override void Update()
		{
		}

		public override void Exit()
		{
		}
	}
	
	public class PlayerCrouchingWalkState : State
	{
		public override string Name { get; set; } = "Walk";
		
		public override void Enter()
		{
			
		}

		public override void Update()
		{
		}

		public override void Exit()
		{
		}
	}
}