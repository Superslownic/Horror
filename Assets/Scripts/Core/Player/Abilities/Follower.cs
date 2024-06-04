using Scripts.Behaviour;
using UnityEngine;

namespace Scripts.Core.Player
{
	public class Follower : Toggleable
	{
		[SerializeField] private Transform _anchor;
		[SerializeField] private Transform _target;
		[SerializeField] private float _speed;

		public void SetAnchor(Transform anchor)
		{
			_anchor = anchor;
		}

		public void SetTarget(Transform target)
		{
			_target = target;
		}

		public void SetSpeed(float speed)
		{
			_speed = speed;
		}

		protected override void OnLateUpdate()
		{
			_anchor.position = Vector3.Lerp(_anchor.position, _target.position, _speed * Time.deltaTime);
		}
	}
}