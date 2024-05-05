using Scripts.Config;
using Scripts.Input;
using Scripts.Units;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Scripts.Core.Player
{
	public class FlashlightAbility : Ability
	{
		[SerializeField] private Light _light;
		[SerializeField] private Transform _lightAnchor;
		[SerializeField] private Transform _raycastAnchor;
		
		[Inject] private readonly InputManager _inputManager;
		[Inject] private readonly GameConfig _gameConfig;

		protected override void OnInitialize()
		{
			_inputManager.Flashlight.performed += HandleButtonClicked;
		}

		protected override void OnUpdate()
		{
			_lightAnchor.rotation = Quaternion.Lerp(_lightAnchor.rotation, _raycastAnchor.rotation, _gameConfig.Player.Flashlight.InterpolationSpeed);
			return;
			if (Physics.Raycast(_raycastAnchor.position, _raycastAnchor.forward, out RaycastHit hit, 1000, _gameConfig.Player.Flashlight.LayerMask))
			{
				_lightAnchor.rotation = Quaternion.Lerp(_lightAnchor.rotation, Quaternion.LookRotation(hit.point - _lightAnchor.position), _gameConfig.Player.Flashlight.InterpolationSpeed);
			}
		}

		private void HandleButtonClicked(InputAction.CallbackContext callbackContext)
		{
			_light.enabled = !_light.enabled;
		}
	}
}