using Scripts.Units;
using UnityEngine;

namespace Scripts.Core.Player
{
	public class PlayerBodyAbility : Ability
	{
		[field: SerializeField] public CapsuleCollider WalkCollider { get; private set; }
		[field: SerializeField] public SphereCollider SwimCollider { get; private set; }
	}
}