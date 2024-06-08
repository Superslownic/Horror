//using Scripts.Reactive;
using Scripts.Units;
using UnityEngine;

namespace Scripts.Core.Player
{
	public class CheckGroundAbility : Ability
	{
		//[SerializeField] private CollisionLink _collisionLink;
		[SerializeField] private LayerMask _groundLayer;
		[SerializeField] private float _heightThreshold;
		[SerializeField] private float _radiusThreshold;

		public bool IsGrounded { get; private set; }
		public float GroundHeight { get; private set; }
		public RaycastHit GroundInfo { get; private set; }
		//public Vector3 ContactNormal { get; private set; }

		private PlayerBodyAbility _playerBodyAbility;

		protected override void OnInitialize()
		{
			base.OnInitialize();
			_playerBodyAbility = Unit.GetAbility<PlayerBodyAbility>();
			//_collisionLink.OnEnter.AddListener(HandleCollision).AddTo(Disposable);
			//_collisionLink.OnStay.AddListener(HandleCollision).AddTo(Disposable);
		}

		protected override void OnUpdate()
		{
			base.OnUpdate();

			float sphereCastRadius = _playerBodyAbility.Radius - _radiusThreshold;

			Physics.SphereCast
			(
				origin: _playerBodyAbility.BodyCenter,
				radius: sphereCastRadius,
				direction: Vector3.down,
				hitInfo: out RaycastHit hitInfo,
				maxDistance: 1000,
				layerMask: _groundLayer
			);

			Vector3 targetFootPosition = hitInfo.point + hitInfo.normal * sphereCastRadius + Vector3.down * sphereCastRadius;
			GroundHeight = _playerBodyAbility.BodyBottom.y - targetFootPosition.y;
			IsGrounded = GroundHeight <= _heightThreshold;
			GroundInfo = hitInfo;
		}

		/*private void HandleCollision(Collision collision)
		{
			for (int i = 0; i < collision.contactCount; i++)
			{
				IsGrounded = true;
				ContactNormal = collision.GetContact(i).normal;
			}
		}*/
	}
}