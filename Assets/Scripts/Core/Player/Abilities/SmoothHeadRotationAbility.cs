using Scripts.Config;
using Scripts.Entities;
using UnityEngine;
using Zenject;

namespace Scripts.Core.Player
{
	public class SmoothHeadRotationAbility : Ability
	{
		[SerializeField] private Transform _anchor;
		[SerializeField] private Transform _target;
		
		[Inject] private readonly GameConfig _gameConfig;

		private Quaternion _rotation;

		protected override void OnInitialize()
		{
			_rotation = _anchor.rotation;
		}

		protected override void OnLateUpdate()
		{
			float angle = Quaternion.Angle(_anchor.rotation, _target.rotation);
			_rotation = Quaternion.Slerp(_rotation, _target.rotation, angle * _gameConfig.Player.Look.RotationInterpolationSpeed * Time.deltaTime);
			_anchor.rotation = _rotation;
		}
		
		//force limit rotation
		/*if (angle > maxDegrees)
		{
			Quaternion fromRotation = _mainAnchor.localRotation;
			Quaternion toRotation = _floatingAnchor.localRotation;

			fromRotation.Normalize();
			toRotation.Normalize();

			Quaternion deltaQuaternion = Quaternion.Inverse(fromRotation) * toRotation;
			deltaQuaternion = Quaternion.RotateTowards(Quaternion.identity, deltaQuaternion, maxDegrees);
			deltaQuaternion.Normalize();

			_floatingAnchor.localRotation = fromRotation * deltaQuaternion;
		}*/
	}
}