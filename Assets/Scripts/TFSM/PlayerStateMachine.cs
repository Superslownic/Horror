using Scripts.Core;
using Scripts.Entities;
using Scripts.Factory;
using Scripts.Input;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace Scripts.TFSM
{
	public class PlayerStateMachine : Ability
	{
		[Inject] private readonly ObjectFactory _objectFactory;
		[Inject] private readonly InputManager _inputManager;

		[ShowInInspector] private StateMachine _stateMachine = new();

		protected override void OnInitialize()
		{
			LadderClimbingAbility ladderClimbingAbility = Unit.GetAbility<LadderClimbingAbility>();
			
			SuperState standing = _objectFactory.CreateInjectedInstance<Standing>(Unit);
			State standingIdle = _objectFactory.CreateInjectedInstance<StandingIdle>(Unit);
			State standingWalk = _objectFactory.CreateInjectedInstance<StandingWalk>(Unit);
			State standingRun = _objectFactory.CreateInjectedInstance<StandingRun>(Unit);
			SuperState crouching = _objectFactory.CreateInjectedInstance<Crouching>(Unit);
			State crouchingIdle = _objectFactory.CreateInjectedInstance<CrouchingIdle>(Unit);
			State crouchingWalk = _objectFactory.CreateInjectedInstance<CrouchingWalk>(Unit);
			SuperState ladderClimbing = _objectFactory.CreateInjectedInstance<LadderClimbing>(Unit);
			State ladderClimbingIdle = _objectFactory.CreateInjectedInstance<LadderClimbingIdle>(Unit);
			State ladderClimbingWalk = _objectFactory.CreateInjectedInstance<LadderClimbingWalk>(Unit);
			State ladderClimbingRun = _objectFactory.CreateInjectedInstance<LadderClimbingRun>(Unit);
			
			_stateMachine.AddState(standing);
			_stateMachine.AddState(standingIdle, parent: standing, transitions: new[] {
				Transition.To<StandingWalk>(when: () => _inputManager.Move.IsPressed()),
				Transition.To<CrouchingIdle>(when: () => _inputManager.Crouch.WasPressedThisFrame()),
			});
			_stateMachine.AddState(standingWalk, parent: standing, transitions: new[] {
				Transition.To<LadderClimbingWalk>(when: ladderClimbingAbility.OnLadderDetected),
				Transition.To<StandingIdle>(when: () => !_inputManager.Move.IsPressed()),
				Transition.To<StandingRun>(when: () => _inputManager.Run.WasPressedThisFrame()),
				Transition.To<CrouchingWalk>(when: () => _inputManager.Crouch.WasPressedThisFrame()),
			});
			_stateMachine.AddState(standingRun, parent: standing, transitions: new[] {
				Transition.To<LadderClimbingRun>(when: ladderClimbingAbility.OnLadderDetected),
				Transition.To<StandingIdle>(when: () => !_inputManager.Move.IsPressed()),
				Transition.To<StandingWalk>(when: () => _inputManager.Run.WasPressedThisFrame()),
				Transition.To<CrouchingWalk>(when: () => _inputManager.Crouch.WasPressedThisFrame()),
			});
			_stateMachine.AddState(crouching);
			_stateMachine.AddState(crouchingIdle, parent: crouching, transitions: new[] {
				Transition.To<StandingIdle>(when: () => _inputManager.Crouch.WasPressedThisFrame()),
				Transition.To<CrouchingWalk>(when: () => _inputManager.Move.IsPressed()),
			});
			_stateMachine.AddState(crouchingWalk, parent: crouching, transitions: new[] {
				Transition.To<LadderClimbingWalk>(when: ladderClimbingAbility.OnLadderDetected),
				Transition.To<CrouchingIdle>(when: () => !_inputManager.Move.IsPressed()),
				Transition.To<StandingWalk>(when: () => _inputManager.Crouch.WasPressedThisFrame()),
				Transition.To<StandingRun>(when: () => _inputManager.Run.WasPressedThisFrame()),
			});
			_stateMachine.AddState(ladderClimbing);
			_stateMachine.AddState(ladderClimbingIdle, parent: ladderClimbing, transitions: new[] {
				Transition.To<LadderClimbingWalk>(when: () => _inputManager.Move.ReadValue<Vector2>().y != 0),
			});
			_stateMachine.AddState(ladderClimbingWalk, parent: ladderClimbing, transitions: new[] {
				Transition.To<LadderClimbingIdle>(when: () => _inputManager.Move.ReadValue<Vector2>().y == 0),
				Transition.To<LadderClimbingRun>(when: () => _inputManager.Run.WasPressedThisFrame()),
				Transition.To<StandingWalk>(when: () => false),
			});
			_stateMachine.AddState(ladderClimbingRun, parent: ladderClimbing, transitions: new[] {
				Transition.To<LadderClimbingIdle>(when: () => _inputManager.Move.ReadValue<Vector2>().y == 0),
				Transition.To<LadderClimbingWalk>(when: () => _inputManager.Run.WasPressedThisFrame()),
				Transition.To<StandingRun>(when: () => false),
			});
			
			_stateMachine.Enter<StandingIdle>();
		}

		private void Update()
		{
			_stateMachine.Update();
		}
	}
}