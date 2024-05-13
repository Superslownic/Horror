using Scripts.Units;
using UnityEngine;

namespace Scripts.Core.Player
{
	public class PlayerBodyAbility : Ability
	{
		[field: SerializeField] public Transform BodyTransform { get; private set; }
		[field: SerializeField] public CapsuleCollider Collider { get; private set; }

		public float HalfHeight => Collider.height * 0.5f;
		public float Radius => Collider.radius;
		public Vector3 BodyCenter => Collider.transform.position + Collider.center;
		public Vector3 BodyBottom => Collider.transform.position + Collider.center + Vector3.down * HalfHeight;
	}
}