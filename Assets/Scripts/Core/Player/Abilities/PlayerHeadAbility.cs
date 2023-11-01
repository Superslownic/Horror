using Scripts.Entities;
using UnityEngine;

namespace Scripts.Core.Player
{
	public class PlayerHeadAbility : Ability
	{
		[field: SerializeField] public Transform HeadStaticAnchor { get; private set; }
		[field: SerializeField] public Transform HeadDetachedAnchor { get; private set; }
		[field: SerializeField] public Transform HeadFloatingAnchor { get; private set; }
	}
}