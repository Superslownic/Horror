using System;
using DG.Tweening;
using Scripts.Units;
using Scripts.Utility.Extensions;
using UnityEngine;

namespace Scripts.Core.Player
{
	public class ChangeHeightAbility : Ability
	{
		private PlayerBodyAbility _playerBodyAbility;
		private PlayerHeadAbility _playerHeadAbility;
		private Tween _tween;
		private ChangeHeightConfig _config;

		protected override void OnInitialize()
		{
			base.OnInitialize();
			_playerBodyAbility = Unit.GetAbility<PlayerBodyAbility>();
			_playerHeadAbility = Unit.GetAbility<PlayerHeadAbility>();
		}

		public void Execute(ChangeHeightConfig config, float duration, Action endCallback = null)
		{
			if (_config == config)
				return;

			_config = config;
			_tween?.Kill();
			_tween = DOTween.Sequence()
				.Join(DOTween.To(() => _playerBodyAbility.Collider.height, value => _playerBodyAbility.Collider.height = value, _config.BodyHeight, duration))
				.Join(DOTween.To(() => _playerBodyAbility.Collider.center, value => _playerBodyAbility.Collider.center = value, new Vector3(0, _config.BodyCenter, 0), duration))
				.Join(DOTween.To(() => _playerHeadAbility.HeadStaticAnchor.localPosition, value => _playerHeadAbility.HeadStaticAnchor.localPosition = value, new Vector3(0, _config.HeadHeight, 0), duration))
				.SetEase(Ease.InOutCubic)
				.AppendCallback(() => endCallback?.Invoke());
		}
	}
}