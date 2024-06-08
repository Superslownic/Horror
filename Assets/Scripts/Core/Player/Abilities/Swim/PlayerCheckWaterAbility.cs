using UnityEngine;

namespace Scripts.Core.Player
{
	public class PlayerCheckWaterAbility : CheckWaterAbility
	{
		public bool CanStand { get; private set; }

		[SerializeField] private LayerMask _layerMask;

		private PlayerHeadAbility _playerHeadAbility;

		protected override void OnInitialize()
		{
			base.OnInitialize();
			_playerHeadAbility = Unit.GetAbility<PlayerHeadAbility>();
		}

		protected override void OnUpdate()
		{
			base.OnUpdate();

			Physics.SphereCast(
				origin: _playerHeadAbility.HeadStaticAnchor.position,
				radius: 0.1f,
				direction: Vector3.down,
				hitInfo: out RaycastHit hitInfo,
				maxDistance: 1000f,
				layerMask: _layerMask,
				queryTriggerInteraction: QueryTriggerInteraction.Ignore);

			float depth = WaterSurfaceHeight - hitInfo.point.y;

			if (depth > 1.7f)
			{
				CanStand = false;
				return;
			}

			CanStand = IsActive && depth <= 1.7f;
		}
	}
}