using Scripts.Units;
using UnityEngine;

namespace Scripts.Core.Player
{
	public class ResizeToFitSurfaceAbility : Ability
	{
		[SerializeField] private LayerMask _groundLayer;

		private PlayerBodyAbility _playerBodyAbility;

		protected override void OnInitialize()
		{
			base.OnInitialize();
			_playerBodyAbility = Unit.GetAbility<PlayerBodyAbility>();
		}

		public void Resize()
		{
			Vector3 origin = _playerBodyAbility.Collider.transform.position + _playerBodyAbility.Collider.center;
			float radius = _playerBodyAbility.Collider.radius - 0.01f;
			Vector3 direction = Vector3.down;

			if (Physics.SphereCast(origin, radius, direction, out RaycastHit hitInfo, 1000f, _groundLayer))
			{
				Vector3 bottomPoint = hitInfo.point + hitInfo.normal * radius + Vector3.down * _playerBodyAbility.Collider.radius;
				Vector3 localTopPoint = _playerBodyAbility.Collider.center + Vector3.up * _playerBodyAbility.Collider.radius;
				Vector3 topPoint = _playerBodyAbility.Collider.transform.TransformPoint(localTopPoint);
				_playerBodyAbility.Collider.center = _playerBodyAbility.Collider.transform.InverseTransformPoint((bottomPoint + topPoint) * 0.5f);
				_playerBodyAbility.Collider.height = topPoint.y - bottomPoint.y;
			}
		}
	}
}