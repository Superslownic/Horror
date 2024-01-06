using DG.Tweening;
using Scripts.Config;
using Scripts.Config.Player;
using Scripts.Entities;
using Scripts.Input;
using UnityEngine;
using Zenject;

namespace Scripts.Core.Player
{
	public class MovementAbility : Ability, IDeactivatableAbility
	{
		[SerializeField] private Transform _lookAnchor;
		[SerializeField] private CharacterController _characterController;
		
		[Inject] private readonly InputManager _inputManager;
		[Inject] private readonly GameConfig _gameConfig;

		public bool IsGrounded { get; private set; }
		public bool IsMoving { get; private set; }
		public Vector3 Velocity { get; private set; }
		public Vector3 NormalizedVelocity { get; private set; }

		private PlayerMovementValues _values;
		private TweenableFloat _speed = new();
		private Vector3 _moveVelocity;

		protected override void OnInitialize()
		{
			_values = _gameConfig.Player.Movement.Walking;
			_speed.Set(_values.Speed);
		}

		protected override void OnDeactivate()
		{
			Velocity = Vector3.zero;
			NormalizedVelocity = Vector3.zero;
		}

		public void ReplaceConfig(PlayerMovementValues config)
		{
			_values = config;
			_speed.Tween(_values.Speed, _gameConfig.Player.Movement.ChangeValuesDuration, Ease.InOutCubic);
		}

		protected override void OnUpdate()
		{
			IsGrounded = _characterController.isGrounded;
			Vector3 groundNormal = GetGroundNormal();

			Vector2 moveInput = _inputManager.Move.ReadValue<Vector2>();
			IsMoving = moveInput.sqrMagnitude > 0;

			_moveVelocity = Vector3.zero;
			Vector3 gravityVelocity = _characterController.isGrounded ? Vector3.zero : Vector3.down * _gameConfig.Player.Movement.Gravity;
			
			//float moveDrag = IsMoving ? 0 : _gameConfig.Player.Movement.GroundedDrag;
			//float fallDrag = _gameConfig.Player.Movement.FallDrag;
			//float resultDrag = 1 - (_characterController.isGrounded ? moveDrag : fallDrag);

			if (IsGrounded && IsMoving)
			{
				Vector3 forwardInputMotion = Vector3.ProjectOnPlane(_lookAnchor.forward, groundNormal).normalized * moveInput.y;
				Vector3 sideInputMotion = Vector3.ProjectOnPlane(_lookAnchor.right, groundNormal).normalized * moveInput.x;

				Vector3 resultInputMotion = (forwardInputMotion + sideInputMotion) * _speed;
				Vector3 clampedInputMotion = Vector3.ClampMagnitude(resultInputMotion, _values.Speed);

				//float factor = Vector3.Angle(_moveVelocity, clampedInputMotion) / 180;
				//float acceleration = Mathf.Lerp(_gameConfig.Player.Movement.Acceleration.Min, _gameConfig.Player.Movement.Acceleration.Max, _gameConfig.Player.Movement.AccelerationCurve.Evaluate(factor));
				
				_moveVelocity = Vector3.Lerp(_moveVelocity, clampedInputMotion, _gameConfig.Player.Movement.Acceleration * Time.deltaTime);
			}
			else
			{
				_moveVelocity = Vector3.Lerp(_moveVelocity, Vector3.zero, _gameConfig.Player.Movement.Deceleration * Time.deltaTime);
			}

			Velocity = _moveVelocity * Time.deltaTime;
			//Velocity *= resultDrag * Time.deltaTime;
			Velocity += gravityVelocity * Time.deltaTime;
			NormalizedVelocity = Velocity / _speed;
			_characterController.Move(Velocity);
		}

		private Vector3 GetGroundNormal()
		{
			Vector3 origin = _characterController.transform.position + _characterController.center;
			float radius = _characterController.radius;
			Vector3 direction = Vector3.down;
			float distance = _characterController.height * 0.5f - _characterController.radius + _gameConfig.Player.Movement.GroundCheckThreshold;
			LayerMask layer = _gameConfig.Player.Movement.FloorLayer;
			
			return Physics.SphereCast(origin, radius, direction, out RaycastHit hit, distance, layer)
				? hit.normal
				: Vector3.zero;
		}
	}
}