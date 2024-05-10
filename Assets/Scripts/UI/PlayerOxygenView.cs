using System.Linq;
using DG.Tweening;
using Scripts.Core;
using Scripts.Core.Player;
using Scripts.Core.Player.Breath;
using Scripts.Reactive;
using Scripts.Units;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI
{
	public class PlayerOxygenView : MonoBehaviour
	{
		[SerializeField] private CanvasGroup _canvasGroup;
		[SerializeField] private float _fadeDuration;
		[SerializeField] private Ease _fadeEase;
		[SerializeField] private Image _fillImage;

		private CompositeDisposable _disposable = new();
		private PlayerOxygenAbility _playerOxygenAbility;
		private Tween _fadeTween;

		private void Start()
		{
			_canvasGroup.alpha = 0;

			Unit playerUnit = UnitFilter.Create().With<PlayerMarkerAbility>().Build(_disposable).First();

			PlayerCheckUnderwaterAbility playerCheckUnderwaterAbility = playerUnit.GetAbility<PlayerCheckUnderwaterAbility>();
			playerCheckUnderwaterAbility.EnterAction.AddListener(HandleUnderWaterEnter).AddTo(_disposable);
			_playerOxygenAbility = playerUnit.GetAbility<PlayerOxygenAbility>();
			_playerOxygenAbility.AmountChangedAction.AddListener(HandleOxygenChanged).AddTo(_disposable);
			_playerOxygenAbility.AmountFullAction.AddListener(HandleAmountFull).AddTo(_disposable);
		}

		private void HandleUnderWaterEnter()
		{
			_fadeTween?.Kill();
			_fadeTween = _canvasGroup.DOFade(endValue: 1, _fadeDuration).SetEase(_fadeEase).Play();
		}

		private void HandleAmountFull()
		{
			_fadeTween?.Kill();
			_fadeTween = _canvasGroup.DOFade(endValue: 0, _fadeDuration).SetEase(_fadeEase).Play();
		}

		private void HandleOxygenChanged()
		{
			_fillImage.fillAmount = _playerOxygenAbility.CurrentAmount / _playerOxygenAbility.MaxAmount;
		}

		private void OnDestroy()
		{
			_disposable.Dispose();
		}
	}
}