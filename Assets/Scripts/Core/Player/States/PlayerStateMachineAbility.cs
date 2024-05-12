using System;
using Scripts.Input;
using Scripts.Reflection;
using Scripts.TFSM;
using Zenject;

namespace Scripts.Core.Player.States
{
	public class PlayerStateMachineAbility : StateMachineAbility
	{
		protected override Type InitialState => TypeCache<StandIdleState>.Value;

		[Inject] private readonly InputManager _inputManager;

		protected override void RegisterStates()
		{
			LadderClimbAbility ladderClimbAbility = Unit.GetAbility<LadderClimbAbility>();
			PlayerCheckWaterAbility checkWaterAbility = Unit.GetAbility<PlayerCheckWaterAbility>();
			ResizeToFitSurfaceAbility resizeToFitSurfaceAbility = Unit.GetAbility<ResizeToFitSurfaceAbility>();

			Condition isMovePressed = new(() => _inputManager.Move.IsPressed());
			Condition crouchWasPressed = new(() => _inputManager.Crouch.WasPressedThisFrame());
			Condition runWasPressed = new(() => _inputManager.Run.WasPressedThisFrame());
			Condition isClimbingOnLadder = new(() => ladderClimbAbility.IsClimbing);
			Condition inWater = new(() => checkWaterAbility.IsInWater);
			Condition canStand = new(() => checkWaterAbility.CanStand);

			RegisterState<StandSuperState>();
			RegisterState<StandIdleState>(transitions: new[] {
				To<StandWalkState>(when: () => isMovePressed),
				To<CrouchIdleState>(when: () => crouchWasPressed),
				To<LadderIdleState>(when: () => isClimbingOnLadder),
				To<SwimIdleState>(when: () => inWater && !canStand),
			});
			RegisterState<StandWalkState>(transitions: new[] {
				To<StandIdleState>(when: () => !isMovePressed),
				To<StandRunState>(when: () => runWasPressed),
				To<CrouchIdleState>(when: () => crouchWasPressed),
				To<LadderWalkState>(when: () => isClimbingOnLadder),
				To<SwimWalkState>(when: () => inWater && !canStand),
			});
			RegisterState<StandRunState>(transitions: new[] {
				To<StandIdleState>(when: () => !isMovePressed),
				To<StandWalkState>(when: () => runWasPressed),
				To<CrouchWalkState>(when: () => crouchWasPressed),
				To<LadderWalkState>(when: () => isClimbingOnLadder),
				To<SwimWalkState>(when: () => inWater && !canStand),
			});

			RegisterState<CrouchSuperState>();
			RegisterState<CrouchIdleState>(transitions: new[] {
				To<CrouchWalkState>(when: () => isMovePressed),
				To<StandIdleState>(when: () => crouchWasPressed),
				To<LadderIdleState>(when: () => isClimbingOnLadder),
				To<SwimIdleState>(when: () => inWater && !canStand),
			});
			RegisterState<CrouchWalkState>(transitions: new[] {
				To<CrouchIdleState>(when: () => !isMovePressed),
				To<StandWalkState>(when: () => crouchWasPressed),
				To<StandRunState>(when: () => runWasPressed),
				To<LadderWalkState>(when: () => isClimbingOnLadder),
				To<SwimWalkState>(when: () => inWater && !canStand),
			});

			RegisterState<LadderSuperState>();
			RegisterState<LadderIdleState>(null);
			RegisterState<LadderWalkState>(null);

			RegisterState<SwimSuperState>();
			RegisterState<SwimIdleState>(transitions: new []
			{
				To<SwimWalkState>(when: () => isMovePressed),
				To<StandIdleState>(when: () => canStand, with: resizeToFitSurfaceAbility.Resize),
			});
			RegisterState<SwimWalkState>(transitions: new []
			{
				To<SwimIdleState>(when: () => !isMovePressed),
				To<StandWalkState>(when: () => canStand, with: resizeToFitSurfaceAbility.Resize),
			});
		}

		private void Update()
		{
			StateMachine.Update();
		}
	}
}