using Scripts.Core.Player.Movement;
using Scripts.Entities;
using UnityEngine;

namespace Scripts.Core.Camera
{
	public class CameraBobbingAbility : Ability
	{
		[SerializeField] private Transform _anchor;
		[SerializeField] private PlayerMovementAbility _movementAbility;
		[SerializeField] private float _threshold;
		[SerializeReference] private ICameraBobbingProcessor[] _processors;

		protected override void OnUpdate()
		{
			if (!_movementAbility.IsGrounded)
			{
				return;
			}
			
			if (_movementAbility.ActualVelocity.magnitude <= _threshold)
			{
				return;
			}
			
			foreach (ICameraBobbingProcessor cameraShakeProcessor in _processors)
			{
				cameraShakeProcessor.Execute(_anchor, _movementAbility.NormalizedActualVelocity.magnitude);
			}
		}
	}
}