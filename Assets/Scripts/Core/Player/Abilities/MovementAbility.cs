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
		
		public Vector3 InputVelocity { get; private set; }
		public Vector3 NormalizedInputVelocity { get; private set; }
		public Vector3 ActualVelocity { get; private set; }
		public Vector3 NormalizedActualVelocity { get; private set; }
		public bool IsGrounded { get; private set; }
		
		private PlayerMovementValues _config;
		private Vector3 _previousPosition;
		private float _speed;
		private Tween _tween;

		protected override void OnInitialize()
		{
			SetConfig(_gameConfig.Player.Movement.Walking);
			ResetValues();
		}

		protected override void OnActivate()
		{
			InputVelocity = Vector3.zero;
			NormalizedInputVelocity = Vector3.zero;
			ActualVelocity = Vector3.zero;
			NormalizedActualVelocity = Vector3.zero;
		}

		public void SetConfig(PlayerMovementValues config)
		{
			_config = config;
			_tween?.Kill();
			_tween = DOTween.Sequence()
				.Join(DOTween
					.To(() => _speed, value => _speed = value, _config.Speed,
						_gameConfig.Player.Movement.ChangeValuesDuration).SetEase(Ease.InOutCubic));
		}

		public void ResetValues()
		{
			_speed = _config.Speed;
		}

		protected override void OnUpdate()
		{
			IsGrounded = CheckGrounded(out Vector3 groundNormal);
			
			if (IsGrounded)
			{
				Vector2 moveInput = _inputManager.Move.ReadValue<Vector2>();

				Vector3 forwardDirection = Vector3.ProjectOnPlane(_lookAnchor.forward, groundNormal).normalized * moveInput.y;
				Vector3 sideDirection = Vector3.ProjectOnPlane(_lookAnchor.right, groundNormal).normalized * moveInput.x;

				Vector3 inputMotion = (forwardDirection + sideDirection) * _speed;
				Vector3 clampedMotion = Vector3.ClampMagnitude(inputMotion, _config.Speed);

				bool isMoving = moveInput.sqrMagnitude > 0;

				Vector3 resultMotion = isMoving ? clampedMotion : Vector3.zero;
				float resultDelta = isMoving ? _gameConfig.Player.Movement.Acceleration : _gameConfig.Player.Movement.Deceleration;
				
				InputVelocity = Vector3.MoveTowards(InputVelocity, resultMotion, resultDelta * Time.deltaTime);
				NormalizedInputVelocity = InputVelocity / _config.Speed;
				_characterController.Move(InputVelocity * Time.deltaTime);
			}
			
			_characterController.Move(Vector3.down * (_gameConfig.Player.Movement.Gravity * Time.deltaTime));

			Vector3 rawActualVelocity = (_characterController.transform.position - _previousPosition) / Time.deltaTime;
			ActualVelocity = Vector3.ClampMagnitude(rawActualVelocity, _config.Speed);
			NormalizedActualVelocity = ActualVelocity / _config.Speed;
			_previousPosition = _characterController.transform.position;
		}

		private bool CheckGrounded(out Vector3 result)
		{
			Vector3 origin = _characterController.transform.position + _characterController.center;
			float radius = _characterController.radius;
			Vector3 direction = Vector3.down;
			float distance = _characterController.height * 0.5f - _characterController.radius + _gameConfig.Player.Movement.GroundCheckThreshold;
			LayerMask layer = _gameConfig.Player.Movement.FloorLayer;
			
			if (Physics.SphereCast(origin, radius, direction, out RaycastHit hit, distance, layer))
			{
				result = hit.normal;
				return true;
			}

			result = default;
			return false;
		}
	}
}