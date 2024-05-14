using Scripts.Units;
using UnityEngine;

namespace Scripts.Core.Player
{
	public class SmoothPositionAbility : Ability
	{
		[SerializeField] private Transform _anchor;
		[SerializeField] private Transform _target;
		[SerializeField] private float _speed;

		protected override void OnUpdate()
		{
			_anchor.position = Vector3.Lerp(_anchor.position, _target.position, _speed * Time.deltaTime);
		}
	}
}