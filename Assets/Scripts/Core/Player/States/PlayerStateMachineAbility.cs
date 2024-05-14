using System;
using Scripts.Input;
using Scripts.Reflection;
using Scripts.TFSM;
using Scripts.Utility.Extensions;
using UnityEngine;
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
			PlayerCheckUnderwaterAbility checkUnderwaterAbility = Unit.GetAbility<PlayerCheckUnderwaterAbility>();
			ResizeToFitSurfaceAbility resizeToFitSurfaceAbility = Unit.GetAbility<ResizeToFitSurfaceAbility>();

			Condition isMovePressed = new(() => _inputManager.Move.IsPressed());
			Condition crouchWasPressed = new(() => _inputManager.Crouch.WasPressedThisFrame());
			Condition runWasPressed = new(() => _inputManager.Run.WasPressedThisFrame());
			Condition isLadderNearby = new(() => ladderClimbAbility.Ladder != null);
			Condition isLadderClimbing = new(() => ladderClimbAbility.IsClimbing);
			Condition isLadderDismounting = new(() => ladderClimbAbility.IsDismounting);
			Condition inWater = new(() => checkWaterAbility.IsInWater);
			Condition canStandInWater = new(() => checkWaterAbility.CanStand);
			Condition isUnderwater = new(() => checkUnderwaterAbility.IsUnderWater);

			RegisterState<StandSuperState>();
			RegisterState<StandIdleState>(transitions: new[] {
				To<StandWalkState>(when: () => isMovePressed),
				To<CrouchIdleState>(when: () => crouchWasPressed),
				To<LadderIdleState>(when: () => isLadderNearby),
				To<SwimIdleState>(when: () => inWater && !canStandInWater),
			});
			RegisterState<StandWalkState>(transitions: new[] {
				To<StandIdleState>(when: () => !isMovePressed),
				To<StandRunState>(when: () => runWasPressed),
				To<CrouchIdleState>(when: () => crouchWasPressed),
				To<LadderWalkState>(when: () => isLadderNearby),
				To<SwimWalkState>(when: () => inWater && !canStandInWater),
			});
			RegisterState<StandRunState>(transitions: new[] {
				To<StandIdleState>(when: () => !isMovePressed),
				To<StandWalkState>(when: () => runWasPressed),
				To<CrouchWalkState>(when: () => crouchWasPressed),
				To<LadderWalkState>(when: () => isLadderNearby),
				To<SwimWalkState>(when: () => inWater && !canStandInWater),
			});

			RegisterState<CrouchSuperState>();
			RegisterState<CrouchIdleState>(transitions: new[] {
				To<CrouchWalkState>(when: () => isMovePressed),
				To<StandIdleState>(when: () => crouchWasPressed),
				To<LadderIdleState>(when: () => isLadderNearby),
				To<SwimIdleState>(when: () => inWater && !canStandInWater),
			});
			RegisterState<CrouchWalkState>(transitions: new[] {
				To<CrouchIdleState>(when: () => !isMovePressed),
				To<StandWalkState>(when: () => crouchWasPressed),
				To<StandRunState>(when: () => runWasPressed),
				To<LadderWalkState>(when: () => isLadderNearby),
				To<SwimWalkState>(when: () => inWater && !canStandInWater),
			});

			RegisterState<LadderSuperState>();
			RegisterState<LadderIdleState>(transitions: new[] {
				To<StandIdleState>(when: () => !isLadderClimbing),
			});
			RegisterState<LadderWalkState>(transitions: new[] {
				To<StandWalkState>(when: () => !isLadderClimbing),
				To<SwimWalkState>(when: () => _inputManager.Jump.WasPressedThisFrame()),
			});

			RegisterState<SwimSuperState>();
			RegisterState<SwimIdleState>(transitions: new[] {
				To<SwimWalkState>(when: () => isMovePressed),
				To<StandIdleState>(when: () => !inWater),
				To<StandIdleState>(when: () => canStandInWater, with: resizeToFitSurfaceAbility.Resize),
			});
			RegisterState<SwimWalkState>(transitions: new [] {
				To<SwimIdleState>(when: () => !isMovePressed),
				To<StandWalkState>(when: () => !inWater),
				To<StandWalkState>(when: () => canStandInWater, with: resizeToFitSurfaceAbility.Resize),
				To<LadderWalkState>(when: () => !isUnderwater && isLadderNearby),
			});
		}
	}
}