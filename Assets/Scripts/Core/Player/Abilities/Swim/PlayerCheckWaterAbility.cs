using UnityEngine;

namespace Scripts.Core.Player
{
	public class PlayerCheckWaterAbility : CheckWaterAbility
	{
		public override bool InWater => base.InWater && !_canStand;

		[SerializeField] private LayerMask _layerMask;

		private PlayerHeadAbility _playerHeadAbility;
		private bool _canStand;

		protected override void OnInitialize()
		{
			base.OnInitialize();
			_playerHeadAbility = Unit.GetAbility<PlayerHeadAbility>();
		}

		protected override void OnUpdate()
		{
			base.OnUpdate();

			bool isHit = Physics.SphereCast(
				origin: _playerHeadAbility.HeadStaticAnchor.position,
				radius: 0.3f,
				direction: Vector3.down,
				hitInfo: out RaycastHit hitInfo,
				maxDistance: 1000f,
				layerMask: _layerMask,
				queryTriggerInteraction: QueryTriggerInteraction.Ignore);

			if (!isHit)
			{
				_canStand = false;
				return;
			}

			float distanceToSurface = WaterSurfaceHeight - hitInfo.point.y;
			_canStand = IsActive && distanceToSurface <= 1.7f;
		}
	}
}