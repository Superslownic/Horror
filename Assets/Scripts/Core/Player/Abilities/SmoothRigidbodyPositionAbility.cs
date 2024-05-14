using UnityEngine;

namespace Scripts.Core.Player
{
	public class SmoothRigidbodyPositionAbility : MonoBehaviour
	{
		[SerializeField] private Rigidbody _rigidbody;
		[SerializeField] private Transform _target;
		[SerializeField] private float _speed;

		private void FixedUpdate()
		{
			_rigidbody.MovePosition(_target.position);
		}
	}
}