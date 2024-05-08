using Scripts.Units;
using UnityEngine;

namespace Scripts.Core.Player
{
	public class CheckGroundAbility : Ability
	{
		[SerializeField] private LayerMask _groundLayer;
		[SerializeField] private float _heightThreshold;

		public bool IsGrounded { get; private set; }
		public RaycastHit GroundInfo { get; private set; }

		private PlayerBodyAbility _playerBodyAbility;

		protected override void OnInitialize()
		{
			base.OnInitialize();
			_playerBodyAbility = Unit.GetAbility<PlayerBodyAbility>();
		}

		protected override void OnUpdate()
		{
			base.OnUpdate();
			IsGrounded = TryGetGroundInfo(_playerBodyAbility.WalkCollider, _groundLayer, _heightThreshold, out RaycastHit info);
			GroundInfo = info;
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