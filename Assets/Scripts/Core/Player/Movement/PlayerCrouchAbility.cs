using DG.Tweening;
using Scripts.Entities;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Scripts.Core.Player.Movement
{
	public class PlayerCrouchAbility : Ability
	{
		[SerializeField] private CharacterController _characterController;
		[SerializeField] private Transform _cameraMainAnchor;

		[Button]
		public void Crouch()
		{
			DOTween.To(() => _characterController.height, value => _characterController.height = value, 1, 0.5f).SetEase(Ease.InOutQuad);
			DOTween.To(() => _characterController.center, value => _characterController.center = value, new Vector3(0, 0.5f, 0), 0.5f).SetEase(Ease.InOutQuad);
			DOTween.To(() => _cameraMainAnchor.localPosition, value => _cameraMainAnchor.localPosition = value, new Vector3(0, 0.8f, 0), 0.5f).SetEase(Ease.InOutQuad);
		}
		
		[Button]
		public void Stand()
		{
			DOTween.To(() => _characterController.height, value => _characterController.height = value, 2, 0.5f).SetEase(Ease.InOutQuad);
			DOTween.To(() => _characterController.center, value => _characterController.center = value, new Vector3(0, 1f, 0), 0.5f).SetEase(Ease.InOutQuad);
			DOTween.To(() => _cameraMainAnchor.localPosition, value => _cameraMainAnchor.localPosition = value, new Vector3(0, 1.8f, 0), 0.5f).SetEase(Ease.InOutQuad);
		}
	}
}