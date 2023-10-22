using DG.Tweening;
using Scripts.Config;
using Scripts.Core.Player.Shake;
using Scripts.Entities;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace Scripts.Core.Player
{
	public class CrouchAbility : Ability
	{
		[SerializeField] private CharacterController _characterController;
		[SerializeField] private Transform _cameraMainAnchor;
		[SerializeField] private ShakeAbility _shakeAbility;

		[Inject] private readonly GameConfig _gameConfig;

		private float _startHeight;
		private float _startCameraHeight;
		private Shaker _crouchShaker;
		private Shaker _standShaker;
		private bool _isCrouching;

		protected override void OnInitialize()
		{
			_startHeight = _characterController.height;
			_startCameraHeight = _cameraMainAnchor.localPosition.y;
			_crouchShaker = _gameConfig.Player.Crouch.CrouchShaker.Clone();
			_standShaker = _gameConfig.Player.Crouch.StandShaker.Clone();
		}

		[Button]
		public void Crouch()
		{
			if (_isCrouching)
			{
				return;
			}

			_isCrouching = true;
			
			DOTween.To(() => _characterController.height, value => _characterController.height = value, 1, _gameConfig.Player.Crouch.Duration).SetEase(Ease.InOutQuad);
			DOTween.To(() => _characterController.center, value => _characterController.center = value, new Vector3(0, _gameConfig.Player.Crouch.TargetHeight * 0.5f, 0), _gameConfig.Player.Crouch.Duration).SetEase(Ease.InOutQuad);
			DOTween.To(() => _cameraMainAnchor.localPosition, value => _cameraMainAnchor.localPosition = value, new Vector3(0, _gameConfig.Player.Crouch.TargetCameraHeight, 0), _gameConfig.Player.Crouch.Duration).SetEase(Ease.InOutQuad);
			
			_shakeAbility.AddShaker(_crouchShaker);
			_crouchShaker.Start();
		}
		
		[Button]
		public void Stand()
		{
			if (!_isCrouching)
			{
				return;
			}

			_isCrouching = false;
			
			DOTween.To(() => _characterController.height, value => _characterController.height = value, _startHeight, _gameConfig.Player.Crouch.Duration).SetEase(Ease.InOutQuad);
			DOTween.To(() => _characterController.center, value => _characterController.center = value, new Vector3(0, _startHeight * 0.5f, 0), _gameConfig.Player.Crouch.Duration).SetEase(Ease.InOutQuad);
			DOTween.To(() => _cameraMainAnchor.localPosition, value => _cameraMainAnchor.localPosition = value, new Vector3(0, _startCameraHeight, 0), _gameConfig.Player.Crouch.Duration).SetEase(Ease.InOutQuad);
			
			_shakeAbility.AddShaker(_standShaker);
			_standShaker.Start();
		}
	}
}