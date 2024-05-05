using UnityEngine;

namespace Scripts.Units
{
	public class RigidbodyAbility : Ability
	{
		[field: SerializeField] public Rigidbody Rigidbody { get; private set; }

		public static implicit operator Rigidbody(RigidbodyAbility rigidbodyAbility) => rigidbodyAbility.Rigidbody;
	}
}