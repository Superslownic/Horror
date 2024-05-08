using Scripts.Config;
using Scripts.Input;
using Scripts.Units;
using UnityEngine;
using Zenject;

namespace Scripts.Core.Player
{
	public class LeanAbility : Ability
	{
		[SerializeField] private Transform _anchor;

		[Inject] private readonly InputManager _inputManager;
		[Inject] private readonly GameConfig _gameConfig;

		private GroundMoveAbility _groundMoveAbility;
		private ChangeVelocityAbility _changeVelocityAbility;
		private CheckGroundAbility _checkGroundAbility;

		protected override void OnInitialize()
		{
			_groundMoveAbility = Unit.GetAbility<GroundMoveAbility>();
			_changeVelocityAbility = Unit.GetAbility<ChangeVelocityAbility>();
			_checkGroundAbility = Unit.GetAbility<CheckGroundAbility>();
		}

		protected override void OnUpdate()
		{
			if (_checkGroundAbility.IsGrounded && _groundMoveAbility.IsMoving)
			{
				float input = _inputManager.Move.ReadValue<Vector2>().x;
				float targetAngle = -input * _changeVelocityAbility.ActualVelocity.normalized.magnitude * _gameConfig.Player.Lean.Angle;
				Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);
				
				_anchor.localRotation = Quaternion.Lerp
				(
					_anchor.localRotation,
					targetRotation,
					_gameConfig.Player.Lean.Force * Time.deltaTime
				);
			}
			else
			{
				_anchor.localRotation = Quaternion.Lerp
				(
					_anchor.localRotation,
					Quaternion.identity,
					_gameConfig.Player.Lean.Force * Time.deltaTime
				);
			}
		}
	}
}