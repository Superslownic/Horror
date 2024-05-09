using Scripts.Units;
using UnityEngine;

namespace Scripts.Core.Player
{
	public class PlayerBodyAbility : Ability
	{
		[field: SerializeField] public CapsuleCollider Collider { get; private set; }
	}
}