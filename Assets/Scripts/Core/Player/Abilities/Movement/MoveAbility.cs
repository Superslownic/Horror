using DG.Tweening;
using Scripts.Config;
using Scripts.Config.Player;
using Scripts.Input;
using Scripts.Units;
using UnityEngine;
using Zenject;

namespace Scripts.Core.Player
{
	public class MoveAbility : Ability
	{
		[SerializeField] private Transform _lookAnchor;
		[SerializeField] private Rigidbody _rigidbody;
		[SerializeField] private float _acceleration;
		[SerializeField] private float _deceleration;
		[SerializeField] private float _airCorrectionMultiplier;
		
		[Inject] private readonly InputManager _inputManager;
		[Inject] private readonly GameConfig _gameConfig;

		public bool IsMoving { get; private set; }
		public Vector3 Velocity => _velocity;
		public Vector3 NormalizedVelocity { get; private set; }

		private CheckGroundAbility _checkGroundAbility;
		private PlayerMovementValues _values;
		private TweenableFloat _speed = new();
		private Vector3 _velocity;

		protected override void OnInitialize()
		{
			_checkGroundAbility = Unit.GetAbility<CheckGroundAbility>();
			_values = _gameConfig.Player.Movement.Walking;
			_speed.Set(_values.Speed);
		}

		protected override void OnDeactivate()
		{
			_velocity = Vector3.zero;
			NormalizedVelocity = Vector3.zero;
		}

		public void ReplaceConfig(PlayerMovementValues config)
		{
			_values = config;
			_speed.Tween(_values.Speed, _gameConfig.Player.Movement.ChangeValuesDuration, Ease.InOutCubic);
		}

		protected override void OnFixedUpdate()
		{
			Vector2 moveInput = _inputManager.Move.ReadValue<Vector2>();
			IsMoving = moveInput.sqrMagnitude > 0;

			if (IsMoving)
			{
				//Vector3 forwardInputMotion = (_checkGroundAbility.IsGrounded ? Vector3.ProjectOnPlane(_lookAnchor.forward, groundNormal).normalized : _lookAnchor.forward) * moveInput.y;
				Vector3 forwardInputMotion = Vector3.ProjectOnPlane(_lookAnchor.forward, Vector3.up).normalized * moveInput.y;

				//Vector3 sideInputMotion = (_checkGroundAbility.IsGrounded ? Vector3.ProjectOnPlane(_lookAnchor.right, groundNormal).normalized : _lookAnchor.right) * moveInput.x ;
				Vector3 sideInputMotion = Vector3.ProjectOnPlane(_lookAnchor.right, Vector3.up).normalized * moveInput.x;

				Vector3 resultInputMotion = (forwardInputMotion + sideInputMotion) * _speed;
				Vector3 clampedInputMotion = Vector3.ClampMagnitude(resultInputMotion, _speed);

				if (_checkGroundAbility.IsGrounded)
				{
					_velocity = Vector3.Lerp(_velocity, clampedInputMotion, _acceleration * Time.deltaTime);
				}
				else
				{
					Vector3 velocity = _velocity + clampedInputMotion * _airCorrectionMultiplier * Time.fixedDeltaTime;
					velocity.y = 0;
					velocity = Vector3.ClampMagnitude(velocity, _speed);
					_velocity.x = velocity.x;
					_velocity.z = velocity.z;
				}
			}
			else 
			{
				if(!_checkGroundAbility.IsGrounded)
					return;

				_velocity = Vector3.Lerp(_velocity, Vector3.zero, _deceleration * Time.deltaTime);
			}

			_velocity.y = _rigidbody.linearVelocity.y;
			NormalizedVelocity = _velocity.normalized;
			_rigidbody.linearVelocity = _velocity;
		}
	}
}