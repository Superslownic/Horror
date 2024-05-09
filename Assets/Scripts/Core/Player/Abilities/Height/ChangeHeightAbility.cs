using DG.Tweening;
using Scripts.Units;
using UnityEngine;

namespace Scripts.Core.Player
{
	public class ChangeHeightAbility : Ability
	{
		[SerializeField] private LayerMask _groundLayer;

		private PlayerBodyAbility _playerBodyAbility;
		private Tween _tween;
		private ChangeHeightConfig _config;

		protected override void OnInitialize()
		{
			base.OnInitialize();
			_playerBodyAbility = Unit.GetAbility<PlayerBodyAbility>();
		}

		public void Execute(ChangeHeightConfig config, float duration)
		{
			if (_config == config)
				return;

			_config = config;
			_tween?.Kill();
			_tween = DOTween.Sequence()
				.Join(DOTween.To(() => _playerBodyAbility.Collider.height, value => _playerBodyAbility.Collider.height = value, _config.Height, duration)
					.SetEase(Ease.InOutQuad))
				.Join(DOTween.To(() => _playerBodyAbility.Collider.center, value => _playerBodyAbility.Collider.center = value, new Vector3(0, _config.Center, 0), duration)
					.SetEase(Ease.InOutQuad));
		}

		public void ResizeToFitSurface()
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