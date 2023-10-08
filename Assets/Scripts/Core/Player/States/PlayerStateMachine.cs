using Scripts.Factory;
using Scripts.FSM.Generic;
using Zenject;

namespace Scripts.Core.Player.States
{
	public class PlayerStateMachine : StateMachine
	{
		[Inject] private readonly IObjectFactory _objectFactory;
		
		protected override State Setup()
		{
			State standing = SetupStandingState();
			State crouching = SetupCrouchingState();
			
			AddTransition(standing, crouching, () => true);
			AddTransition(crouching, standing, () => true);
			
			return standing;
		}

		private State SetupStandingState()
		{
			CompositeState standing = new();
			
			State idle = _objectFactory.CreateInjectedInstance<PlayerStandingIdleState>();
			State walk = _objectFactory.CreateInjectedInstance<PlayerStandingWalkState>();
			State run = _objectFactory.CreateInjectedInstance<PlayerStandingRunState>();
			
			standing.AddTransition(idle, walk, null);
			standing.AddTransition(idle, run, null);
			standing.AddTransition(walk, idle, null);
			standing.AddTransition(walk, run, null);

			return standing;
		}
		
		private State SetupCrouchingState()
		{
			CompositeState crouching = new();
			
			State idle = _objectFactory.CreateInjectedInstance<PlayerCrouchingIdleState>();
			State walk = _objectFactory.CreateInjectedInstance<PlayerCrouchingWalkState>();
			
			crouching.AddTransition(idle, walk, null);
			crouching.AddTransition(walk, idle, null);

			return crouching;
		}
	}
	
	public class PlayerStandingIdleState : State
	{
		public override void OnEnter()
		{
			
		}

		public override void OnUpdate()
		{
		}

		public override void OnExit()
		{
		}
	}

	public class PlayerStandingWalkState : State
	{
		public override void OnEnter()
		{
			
		}

		public override void OnUpdate()
		{
		}

		public override void OnExit()
		{
		}
	}
	
	public class PlayerStandingRunState : State
	{
		public override void OnEnter()
		{
			
		}

		public override void OnUpdate()
		{
		}

		public override void OnExit()
		{
		}
	}
	
	public class PlayerCrouchingIdleState : State
	{
		public override void OnEnter()
		{
			
		}

		public override void OnUpdate()
		{
		}

		public override void OnExit()
		{
		}
	}
	
	public class PlayerCrouchingWalkState : State
	{
		public override void OnEnter()
		{
			
		}

		public override void OnUpdate()
		{
		}

		public override void OnExit()
		{
		}
	}
}