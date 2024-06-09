using DG.Tweening;
using Scripts.Configs;
using Scripts.Configs.Player;
using Scripts.Input;
using Scripts.Units;
using UnityEngine;
using Zenject;
using Scripts.Reactive;

namespace Scripts.Core.Player
{
	public class GroundMoveAbility : Ability
	{
		[SerializeField] private CollisionLink _collisionLink;
		[SerializeField] private Transform _lookAnchor;
		[SerializeField] private Rigidbody _rigidbody;
		[SerializeField] private float _acceleration;
		[SerializeField] private float _deceleration;
		[SerializeField] private float _airCorrectionMultiplier;
		[SerializeField] private float _maxSlopeAngle;
		[SerializeField] private float _jumpHeight;
		[SerializeField] private float _groundStickPreventionDelay;
		[SerializeField] private float _airJumpTime;
		[SerializeField] private float _allowedJumpCount;

		[Inject] private readonly InputManager _inputManager;
		[Inject] private readonly GameConfig _gameConfig;

		public bool IsGrounded => _groundContactCount > 0;
		public bool IsMoving { get; private set; }

		private PlayerMovementValues _values;
		private TweenableFloat _maxSpeed = new();
		private int _groundContactCount;
		private Vector3 _contactNormal;
		private Vector3 _velocity;
		private bool _isGroundedLastFrame;
		private float _lastJumpTime;
		private float _lastUngroundedTime;
		private float _jumpCount;

		protected override void OnInitialize()
		{
			_collisionLink.OnEnter.AddListener(HandleCollision).AddTo(Disposable);
			_collisionLink.OnStay.AddListener(HandleCollision).AddTo(Disposable);
			_values = _gameConfig.Player.Movement.Walking;
			_maxSpeed.Set(_values.Speed);
		}

		protected override void OnDeactivate()
		{
			ReplaceConfig(_gameConfig.Player.Movement.Walking);
		}

		public void ReplaceConfig(PlayerMovementValues config)
		{
			_values = config;
			_maxSpeed.Tween(_values.Speed, _gameConfig.Player.Movement.ChangeValuesDuration, Ease.InOutCubic);
		}

		protected override void OnFixedUpdate()
		{
			if (IsGrounded && !_isGroundedLastFrame)
				OnLanded();

			if (!IsGrounded && _isGroundedLastFrame)
				OnTookOff();

			_velocity = _rigidbody.linearVelocity;

			CalculateDesiredVelocity();
			ApplyMove();
			ApplyGravity();
			ApplyJump();
			ApplyAirCorrection();

			_rigidbody.linearVelocity = _velocity;
			_isGroundedLastFrame = IsGrounded;

			_groundContactCount = 0;
			_contactNormal = Vector3.zero;
		}

		private void HandleCollision(Collision collision)
		{
			for (int i = 0; i < collision.contactCount; i++)
			{
				Vector3 normal = collision.GetContact(i).normal;

				if(normal.y < Mathf.Cos(_maxSlopeAngle * Mathf.Deg2Rad))
					continue;

				_groundContactCount++;
				_contactNormal += normal;
			}

			_contactNormal = _contactNormal.normalized;
		}

		private void OnLanded()
		{
			_jumpCount = 0;
		}

		private void OnTookOff()
		{
			_lastUngroundedTime = Time.time;
		}

		private Vector3 CalculateDesiredVelocity()
		{
			Vector2 moveInput = _inputManager.Move.ReadValue<Vector2>();
			Vector3 projectedForward = Vector3.ProjectOnPlane(_lookAnchor.forward, Vector3.up).normalized;
			Vector3 projectedRight = Vector3.ProjectOnPlane(_lookAnchor.right, Vector3.up).normalized;
			Vector3 forwardInputMotion = Vector3.ProjectOnPlane(projectedForward, Vector3.ProjectOnPlane(_contactNormal, projectedRight)).normalized * moveInput.y;
			Vector3 sideInputMotion = Vector3.ProjectOnPlane(projectedRight, Vector3.ProjectOnPlane(_contactNormal, projectedForward)).normalized * moveInput.x;
			Vector3 resultInputMotion = (forwardInputMotion + sideInputMotion) * _maxSpeed;
			return Vector3.ClampMagnitude(resultInputMotion, _maxSpeed);
		}

		private void ApplyMove()
		{
			Vector2 moveInput = _inputManager.Move.ReadValue<Vector2>();

			IsMoving = IsGrounded && moveInput.sqrMagnitude > 0;

			if (!IsGrounded)
				return;

			_velocity = Vector3.Lerp(_velocity, CalculateDesiredVelocity(), (IsMoving ? _acceleration : _deceleration) * Time.deltaTime);
		}

		private void ApplyGravity()
		{
			if(!IsGrounded)
				_rigidbody.AddForce(Physics.gravity);

			if(!IsGrounded || Time.time - _lastJumpTime < _groundStickPreventionDelay)
				_velocity.y = _rigidbody.linearVelocity.y;
		}

		private void ApplyJump()
		{
			if (!IsGrounded && (Time.time - _lastUngroundedTime > _airJumpTime || _jumpCount >= _allowedJumpCount))
				return;

			if(!_inputManager.Jump.WasPressedThisFrame())
				return;

			_lastJumpTime = Time.time;
			_velocity.y = Mathf.Sqrt(-2f * Physics.gravity.y * _jumpHeight);
			_jumpCount++;
		}

		private void ApplyAirCorrection()
		{
			if (IsGrounded)
				return;

			Vector3 airControlVelocity = CalculateDesiredVelocity() * _airCorrectionMultiplier;
			_rigidbody.AddForce(airControlVelocity);
		}
	}
}