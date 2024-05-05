using Scripts.Config;
using Scripts.Units;
using UnityEngine;
using Zenject;

namespace Scripts.Core.Player
{
	public class SmoothHeadRotationAbility : Ability
	{
		[SerializeField] private Transform _anchor;
		[SerializeField] private Transform _target;
		
		[Inject] private readonly GameConfig _gameConfig;

		protected override void OnLateUpdate()
		{
			_anchor.rotation = _target.rotation;
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