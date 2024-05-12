using DG.Tweening;
using Scripts.Configs;
using Scripts.Configs.Player;
using Scripts.Input;
using Scripts.Units;
using UnityEngine;
using Zenject;

namespace Scripts.Core.Player
{
	public class GroundMoveAbility : Ability
	{
		[SerializeField] private Transform _lookAnchor;
		[SerializeField] private Rigidbody _rigidbody;
		[SerializeField] private float _acceleration;
		[SerializeField] private float _deceleration;
		[SerializeField] private float _airCorrectionMultiplier;
		
		[Inject] private readonly InputManager _inputManager;
		[Inject] private readonly GameConfig _gameConfig;

		public bool IsMoving { get; private set; }

		private CheckGroundAbility _checkGroundAbility;
		private PlayerMovementValues _values;
		private TweenableFloat _speed = new();
		private ChangeVelocityAbility _changeVelocityAbility;

		protected override void OnInitialize()
		{
			_checkGroundAbility = Unit.GetAbility<CheckGroundAbility>();
			_changeVelocityAbility = Unit.GetAbility<ChangeVelocityAbility>();
			_values = _gameConfig.Player.Movement.Walking;
			_speed.Set(_values.Speed);
		}

		protected override void OnDeactivate()
		{
			ReplaceConfig(_gameConfig.Player.Movement.Walking);
		}

		public void ReplaceConfig(PlayerMovementValues config)
		{
			_values = config;
			_speed.Tween(_values.Speed, _gameConfig.Player.Movement.ChangeValuesDuration, Ease.InOutCubic);
		}

		protected override void OnUpdate()
		{
			Vector2 moveInput = _inputManager.Move.ReadValue<Vector2>();
			Vector3 velocity = _changeVelocityAbility.TargetVelocity;
			IsMoving = moveInput.sqrMagnitude > 0;

			if (IsMoving)
			{
				Vector3 forwardInputMotion = Vector3.ProjectOnPlane(_lookAnchor.forward, Vector3.up).normalized * moveInput.y;
				Vector3 sideInputMotion = Vector3.ProjectOnPlane(_lookAnchor.right, Vector3.up).normalized * moveInput.x;
				Vector3 resultInputMotion = (forwardInputMotion + sideInputMotion) * _speed;
				Vector3 clampedInputMotion = Vector3.ClampMagnitude(resultInputMotion, _speed);

				if (_checkGroundAbility.IsGrounded)
				{
					_changeVelocityAbility.ChangeSpeed.Set(_acceleration);
					velocity = clampedInputMotion;
				}
				else
				{
					velocity = _changeVelocityAbility.ActualVelocity + clampedInputMotion * (_airCorrectionMultiplier * Time.fixedDeltaTime);
					velocity.y = 0;
					velocity = Vector3.ClampMagnitude(velocity, _speed);
				}
			}
			else
			{
				if(!_checkGroundAbility.IsGrounded)
					return;

				_changeVelocityAbility.ChangeSpeed.Set(_deceleration);
				velocity = Vector3.zero;
			}

			_changeVelocityAbility.SetTargetVelocity(x: velocity.x, z: velocity.z);
		}
	}
}