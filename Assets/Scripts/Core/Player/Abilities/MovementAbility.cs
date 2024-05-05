using DG.Tweening;
using Scripts.Config;
using Scripts.Config.Player;
using Scripts.Input;
using Scripts.Units;
using UnityEngine;
using Zenject;

namespace Scripts.Core.Player
{
	public class MovementAbility : Ability
	{
		[SerializeField] private Transform _lookAnchor;
		[SerializeField] private Rigidbody _rigidbody;
		[SerializeField] private float _groundedDrag;
		[SerializeField] private CapsuleCollider _collider;
		[SerializeField] private LayerMask _layerMask;
		[SerializeField] private float _groundCheckThreshold;
		
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

		protected override void OnFixedUpdate()
		{
			IsGrounded = TryGetGroundInfo(_collider, _layerMask, _groundCheckThreshold, out RaycastHit info);
			//_rigidbody.drag = IsGrounded ? _groundedDrag : 0;

			if(!IsGrounded)
				return;

			Vector3 groundNormal = info.normal;
			Vector2 moveInput = _inputManager.Move.ReadValue<Vector2>();
			IsMoving = moveInput.sqrMagnitude > 0;

			if (IsMoving)
			{
				Vector3 forwardInputMotion = Vector3.ProjectOnPlane(_lookAnchor.forward, groundNormal).normalized * moveInput.y;
				Vector3 sideInputMotion = Vector3.ProjectOnPlane(_lookAnchor.right, groundNormal).normalized * moveInput.x;

				Vector3 resultInputMotion = (forwardInputMotion + sideInputMotion) * _speed;
				Vector3 clampedInputMotion = Vector3.ClampMagnitude(resultInputMotion, _speed);

				Velocity = clampedInputMotion;
			}
			else
			{
				Velocity = Vector3.zero;
			}

			NormalizedVelocity = Velocity.normalized;
			//_rigidbody.AddForce(Velocity);
			_rigidbody.velocity = Velocity;
		}

		private bool TryGetGroundInfo(CapsuleCollider collider, LayerMask layerMask, float threshold, out RaycastHit info)
		{
			Vector3 origin = collider.transform.position + collider.center;
			float radius = collider.radius - 0.01f;
			Vector3 direction = Vector3.down;
			float distance = collider.height * 0.5f - collider.radius + threshold;
			return Physics.SphereCast(origin, radius, direction, out info, distance, layerMask);
		}
	}
}