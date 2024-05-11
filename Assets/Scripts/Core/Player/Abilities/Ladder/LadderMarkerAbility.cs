using Scripts.Units;
using UnityEngine;

namespace Scripts.Core.Player
{
	public class LadderMarkerAbility : Ability
	{
		[field: SerializeField] public Transform TopMountPoint { get; private set; }
		[field: SerializeField] public Transform TopDismountPoint { get; private set; }
		[field: SerializeField] public Transform BottomMountPoint { get; private set; }
		[field: SerializeField] public Transform BottomDismountPoint { get; private set; }
		[field: SerializeField] public Transform RotationTransform { get; private set; }
	}
}