using Scripts.Entities;
using UnityEngine;

namespace Scripts.Core.Camera
{
	public class CameraSmoothAbility : Ability
	{
		[SerializeField] private Transform _anchor;
		[SerializeField] private Transform _target;
		[SerializeField] private float _speed;

		private Vector3 _position;

		protected override void OnInitialize()
		{
			_position = _anchor.position;
		}

		protected override void OnUpdate()
		{
			_position = Vector3.Lerp(_position, _target.position, _speed * Time.deltaTime);
			_anchor.position = _position;
		}
	}
}