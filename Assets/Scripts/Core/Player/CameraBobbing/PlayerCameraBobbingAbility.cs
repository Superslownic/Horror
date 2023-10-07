using Scripts.Config;
using Scripts.Core.Player.Movement;
using Scripts.Entities;
using UnityEngine;
using Zenject;

namespace Scripts.Core.Player
{
	public class PlayerCameraBobbingAbility : DeactivatableAbility
	{
		[SerializeField] private Camera _camera;
		[SerializeField] private PlayerMovementAbility _movementAbility;
		[SerializeReference] private ICameraBobbingProcessor[] _processors;

		[Inject] private readonly GameConfig _gameConfig;

		protected override void OnUpdate()
		{
			foreach (ICameraBobbingProcessor cameraShakeProcessor in _processors)
			{
				cameraShakeProcessor.Execute(_camera.transform, _movementAbility.Velocity.magnitude / _gameConfig.Player.Movement.Speed);
			}
		}
	}
}