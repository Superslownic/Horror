using Scripts.Entities;
using UnityEngine;

namespace Scripts.Core.Camera
{
	public class CameraSmoothAbility : Ability
	{
		[SerializeField] private Transform _anchor;
		[SerializeField] private Transform _target;
		[SerializeField] private float _speed;

		protected override void OnUpdate()
		{
			_anchor.position = Vector3.Lerp(_anchor.position, _target.position, _speed * Time.deltaTime);
			_anchor.rotation = _target.rotation;
		}
	}
}