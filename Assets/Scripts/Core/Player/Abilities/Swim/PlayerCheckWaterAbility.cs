using Scripts.Units;
using UnityEngine;

namespace Scripts.Core.Player
{
	public class PlayerCheckWaterAbility : CheckWaterAbility
	{
		public bool CanStand { get; private set; }

		[SerializeField] private LayerMask _layerMask;

		private PlayerHeadAbility _playerHeadAbility;
		private RigidbodyAbility _rigidbodyAbility;

		protected override void OnInitialize()
		{
			base.OnInitialize();
			_playerHeadAbility = Unit.GetAbility<PlayerHeadAbility>();
			_rigidbodyAbility = Unit.GetAbility<RigidbodyAbility>();
		}

		/*protected override void OnFixedUpdate()
		{
			base.OnFixedUpdate();
			if(InWater)
			{
				if (_playerHeadAbility.HeadStaticAnchor.position.y > WaterSurfaceHeight)
				{
					float distance = _playerHeadAbility.HeadStaticAnchor.position.y - _rigidbodyAbility.Rigidbody.transform.position.y;
					Vector3 position = _rigidbodyAbility.Rigidbody.transform.position;
					position.y = WaterSurfaceHeight - distance;
					_rigidbodyAbility.Rigidbody.transform.position = position;
				}
			}
		}*/

		protected override void OnUpdate()
		{
			base.OnUpdate();

			Physics.SphereCast(
				origin: _playerHeadAbility.HeadStaticAnchor.position,
				radius: 0.3f,
				direction: Vector3.down,
				hitInfo: out RaycastHit hitInfo,
				maxDistance: 1000f,
				layerMask: _layerMask,
				queryTriggerInteraction: QueryTriggerInteraction.Ignore);

			float distanceToSurface = WaterSurfaceHeight - hitInfo.point.y;

			if (distanceToSurface > 1.7f)
			{
				CanStand = false;
				return;
			}

			CanStand = IsActive && distanceToSurface <= 1.7f;
		}
	}
}