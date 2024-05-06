using Scripts.Units;
using UnityEngine;

namespace Scripts.Core.Player
{
	public class SmoothHeadRotationAbility : Ability
	{
		[SerializeField] private Transform _anchor;
		[SerializeField] private Transform _target;
		[SerializeField] private float _speed;

		private Quaternion _rotation;

		protected override void OnInitialize()
		{
			_rotation = _anchor.rotation;
		}

		protected override void OnLateUpdate()
		{
			float angle = Quaternion.Angle(_anchor.rotation, _target.rotation);
			_rotation = Quaternion.Slerp(_rotation, _target.rotation, angle * _speed * Time.deltaTime);
			_anchor.rotation = _rotation;
		}
	}
}