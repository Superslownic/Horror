using DG.Tweening;
using Scripts.Units;
using UnityEngine;

namespace Scripts.Core.Player
{
	public class ChangeHeightAbility : Ability
	{
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
	}
}