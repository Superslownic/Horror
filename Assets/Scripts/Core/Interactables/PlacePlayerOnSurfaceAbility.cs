using Scripts.Core.Player;
using Scripts.Units;
using UnityEngine;

namespace Scripts.Core
{
	public class PlacePlayerOnSurfaceAbility : Ability
	{
		[SerializeField] private float _maxDistance;
		[SerializeField] private LayerMask _surfaceLayer;
		
		private PlayerBodyAbility _playerBodyAbility;

		protected override void OnInitialize()
		{
			_playerBodyAbility = Unit.GetAbility<PlayerBodyAbility>();
		}

		public void Place(Vector3 point)
		{
			float radius = _playerBodyAbility.WalkCollider.radius;
			Physics.SphereCast(point + Vector3.up, radius, Vector3.down, out RaycastHit hit, _maxDistance, _surfaceLayer);
			_playerBodyAbility.WalkCollider.transform.position = hit.point + hit.normal * radius + Vector3.down * radius;
		}
	}
}