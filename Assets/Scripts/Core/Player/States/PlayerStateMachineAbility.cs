using System;
using Scripts.Reflection;
using Scripts.TFSM;

namespace Scripts.Core.Player.States
{
	public class PlayerStateMachineAbility : StateMachineAbility
	{
		protected override Type InitialState => TypeCache<StandIdleState>.Value;

		protected override void RegisterStates()
		{
			RegisterState<StandSuperState>();
			{
				RegisterState<StandIdleState>();
				{
					RegisterTransition<StandIdleToStandWalkTransition>();
					RegisterTransition<StandIdleToCrouchIdleTransition>();
					RegisterTransition<StandIdleToLadderIdleTransition>();
					RegisterTransition<StandIdleToSwimIdleTransition>();
				}

				RegisterState<StandWalkState>();
				{
					RegisterTransition<StandWalkToStandIdleTransition>();
					RegisterTransition<StandWalkToStandRunTransition>();
					RegisterTransition<StandWalkToCrouchWalkTransition>();
					RegisterTransition<StandWalkToLadderWalkTransition>();
					RegisterTransition<StandWalkToSwimWalkTransition>();
				}

				RegisterState<StandRunState>();
				{
					RegisterTransition<StandRunToStandIdleTransition>();
					RegisterTransition<StandRunToStandWalkTransition>();
					RegisterTransition<StandRunToCrouchWalkTransition>();
					RegisterTransition<StandRunToLadderRunTransition>();
					RegisterTransition<StandRunToSwimWalkTransition>();
				}
			}

			RegisterState<CrouchSuperState>();
			{
				RegisterState<CrouchIdleState>();
				{
					RegisterTransition<CrouchIdleToCrouchWalkTransition>();
					RegisterTransition<CrouchIdleToStandIdleTransition>();
					RegisterTransition<CrouchIdleToLadderIdleTransition>();
					RegisterTransition<CrouchIdleToSwimIdleTransition>();
				}

				RegisterState<CrouchWalkState>();
				{
					RegisterTransition<CrouchWalkToCrouchIdleTransition>();
					RegisterTransition<CrouchWalkToStandWalkTransition>();
					RegisterTransition<CrouchWalkToStandRunTransition>();
					RegisterTransition<CrouchWalkToLadderWalkTransition>();
					RegisterTransition<CrouchWalkToSwimWalkTransition>();
				}
			}

			RegisterState<LadderSuperState>();
			RegisterState<LadderIdleState>();
			RegisterState<LadderWalkState>();
			RegisterState<LadderRunState>();
		}

		/*protected override void OnInitialize()
		{
			DiContainer diContainer = new DiContainer(_diContainer);
			diContainer.BindInstance(Unit).AsSingle();
			StateMachineFactory stateMachineFactory = diContainer.Instantiate<StateMachineFactory>();

			SuperState standing = diContainer.Instantiate<Standing>();
			State standingIdle = diContainer.Instantiate<StandingIdle>();
			State standingWalk = diContainer.Instantiate<StandingWalk>();
			State standingRun = diContainer.Instantiate<StandingRun>();

			SuperState crouching = diContainer.Instantiate<Crouching>();
			State crouchingIdle = diContainer.Instantiate<CrouchingIdle>();
			State crouchingWalk = diContainer.Instantiate<CrouchingWalk>();

			SuperState ladderClimbing = diContainer.Instantiate<LadderSuperState>();
			State ladderClimbingIdle = diContainer.Instantiate<LadderClimbingIdle>();
			State ladderClimbingWalk = diContainer.Instantiate<LadderClimbingWalk>();
			State ladderClimbingRun = diContainer.Instantiate<LadderClimbingRun>();

			StateMachine.AddState(standing);
			StateMachine.AddState(standingIdle, parent: standing, transitions: new[] {
				Transition.To<StandingWalk>(when: () => _inputManager.Move.IsPressed()),
				Transition.To<CrouchingIdle>(when: () => _inputManager.Crouch.WasPressedThisFrame()),
			});
			StateMachine.AddState(standingWalk, parent: standing, transitions: new[] {
				diContainer.Instantiate<StandWalkToLadderTransition>(),
				Transition.To<StandingIdle>(when: () => !_inputManager.Move.IsPressed()),
				Transition.To<StandingRun>(when: () => _inputManager.Run.WasPressedThisFrame()),
				Transition.To<CrouchingWalk>(when: () => _inputManager.Crouch.WasPressedThisFrame()),
			});
			StateMachine.AddState(standingRun, parent: standing, transitions: new[] {
				Transition.To<LadderClimbingRun>(when: ladderClimbingAbility.LadderDetectedAction),
				Transition.To<StandingIdle>(when: () => !_inputManager.Move.IsPressed()),
				Transition.To<StandingWalk>(when: () => _inputManager.Run.WasPressedThisFrame()),
				Transition.To<CrouchingWalk>(when: () => _inputManager.Crouch.WasPressedThisFrame()),
			});
			StateMachine.AddState(crouching);
			StateMachine.AddState(crouchingIdle, parent: crouching, transitions: new[] {
				Transition.To<StandingIdle>(when: () => _inputManager.Crouch.WasPressedThisFrame()),
				Transition.To<CrouchingWalk>(when: () => _inputManager.Move.IsPressed()),
			});
			StateMachine.AddState(crouchingWalk, parent: crouching, transitions: new[] {
				Transition.To<LadderClimbingWalk>(when: ladderClimbingAbility.LadderDetectedAction),
				Transition.To<CrouchingIdle>(when: () => !_inputManager.Move.IsPressed()),
				Transition.To<StandingWalk>(when: () => _inputManager.Crouch.WasPressedThisFrame()),
				Transition.To<StandingRun>(when: () => _inputManager.Run.WasPressedThisFrame()),
			});
			StateMachine.AddState(ladderClimbing);
			StateMachine.AddState(ladderClimbingIdle, parent: ladderClimbing, transitions: new[] {
				Transition.To<LadderClimbingWalk>(when: () => _inputManager.Move.ReadValue<Vector2>().y != 0),
			});
			StateMachine.AddState(ladderClimbingWalk, parent: ladderClimbing, transitions: new[] {
				Transition.To<LadderClimbingIdle>(when: () => _inputManager.Move.ReadValue<Vector2>().y == 0),
				Transition.To<LadderClimbingRun>(when: () => _inputManager.Run.WasPressedThisFrame()),
				Transition.To<StandingWalk>(when: () => false),
			});
			StateMachine.AddState(ladderClimbingRun, parent: ladderClimbing, transitions: new[] {
				Transition.To<LadderClimbingIdle>(when: () => _inputManager.Move.ReadValue<Vector2>().y == 0),
				Transition.To<LadderClimbingWalk>(when: () => _inputManager.Run.WasPressedThisFrame()),
				Transition.To<StandingRun>(when: () => false),
			});

			StateMachine = stateMachineFactory.StateMachine;
			StateMachine.Enter<StandingIdle>();
		}*/

		private void Update()
		{
			StateMachine.Update();
		}
	}
}