using UnityEngine;

namespace Scripts.Units
{
	public class RigidbodyAbility : Ability
	{
		[field: SerializeField] public Rigidbody Rigidbody { get; private set; }
		[field: SerializeField] public Transform CenterOfMass { get; private set; }

		public static implicit operator Rigidbody(RigidbodyAbility rigidbodyAbility) => rigidbodyAbility.Rigidbody;

		protected override void OnInitialize()
		{
			base.OnInitialize();

			if (CenterOfMass != null)
			{
				Rigidbody.automaticCenterOfMass = false;
				Rigidbody.centerOfMass = CenterOfMass.localPosition;
			}
			else
			{
				Rigidbody.automaticCenterOfMass = true;
			}
		}
	}
}