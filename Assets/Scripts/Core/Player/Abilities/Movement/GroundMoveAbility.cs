using System;
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

		[Inject] private readonly InputManager _inputManager;
		[Inject] private readonly GameConfig _gameConfig;

		public bool IsGrounded => GroundContactCount > 0;
		public int GroundContactCount { get; private set; }
		public Vector3 ContactNormal { get; private set; }
		public bool IsMoving { get; private set; }

		private PlayerMovementValues _values;
		private TweenableFloat _maxSpeed = new();
		private ChangeVelocityAbility _changeVelocityAbility;
		private Vector3 _velocity;
		private float _inAirTimer;

		protected override void OnInitialize()
		{
			_collisionLink.OnEnter.AddListener(HandleCollision).AddTo(Disposable);
			_collisionLink.OnStay.AddListener(HandleCollision).AddTo(Disposable);
			_changeVelocityAbility = Unit.GetAbility<ChangeVelocityAbility>();
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
			Vector2 moveInput = _inputManager.Move.ReadValue<Vector2>();
			_velocity = _rigidbody.linearVelocity;

			IsMoving = moveInput.sqrMagnitude > 0;

			if(IsGrounded)
			{
				if (IsMoving)
				{
					Vector3 projectedForward = Vector3.ProjectOnPlane(_lookAnchor.forward, Vector3.up).normalized;
					Vector3 projectedRight = Vector3.ProjectOnPlane(_lookAnchor.right, Vector3.up).normalized;
					Vector3 forwardInputMotion = Vector3.ProjectOnPlane(projectedForward, Vector3.ProjectOnPlane(ContactNormal, projectedRight)).normalized * moveInput.y;
					Vector3 sideInputMotion = Vector3.ProjectOnPlane(projectedRight, Vector3.ProjectOnPlane(ContactNormal, projectedForward)).normalized * moveInput.x;
					Vector3 resultInputMotion = (forwardInputMotion + sideInputMotion) * _maxSpeed;
					Vector3 clampedInputMotion = Vector3.ClampMagnitude(resultInputMotion, _maxSpeed);
					_velocity = clampedInputMotion;
				}
				else
				{
					_velocity = IsGrounded ? Vector3.zero : _rigidbody.linearVelocity;
				}
			}

			_rigidbody.useGravity = !IsGrounded;

			if (_inAirTimer > 0)
			{
				_inAirTimer -= Time.deltaTime;
			}

			if(!IsGrounded || _inAirTimer > 0)
				_velocity.y = _rigidbody.linearVelocity.y;

			_rigidbody.linearVelocity = _velocity;

			GroundContactCount = 0;
			ContactNormal = Vector3.up;
		}

		public void SetInAir()
		{
			_inAirTimer = 0.1f;
		}

		private void HandleCollision(Collision collision)
		{
			for (int i = 0; i < collision.contactCount; i++)
			{
				if(collision.GetContact(i).normal.y >= Mathf.Cos(_maxSlopeAngle * Mathf.Deg2Rad))
				{
					GroundContactCount++;
					ContactNormal += collision.GetContact(i).normal;
				}
			}

			if(GroundContactCount > 1)
				ContactNormal.Normalize();
		}
	}
}