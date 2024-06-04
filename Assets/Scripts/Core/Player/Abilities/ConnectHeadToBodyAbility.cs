using DG.Tweening;
using Scripts.Units;
using UnityEngine;

namespace Scripts.Core.Player
{
	public class ConnectHeadToBodyAbility : Ability
	{
		[SerializeField] private float _duration;

		private PlayerHeadAbility _playerHeadAbility;
		private Tween _tween;

		protected override void OnInitialize()
		{
			_playerHeadAbility = Unit.GetAbility<PlayerHeadAbility>();
		}

		public void Execute(float duration = float.NaN)
		{
			_tween?.Kill();
			_playerHeadAbility.HeadDetachedAnchor.SetParent(_playerHeadAbility.HeadStaticAnchor);
			_tween = _playerHeadAbility.HeadDetachedAnchor
				.DOLocalMove(endValue: Vector3.zero, !float.IsNaN(duration) ? duration : _duration)
				.SetEase(Ease.InOutCubic);
		}
	}
}