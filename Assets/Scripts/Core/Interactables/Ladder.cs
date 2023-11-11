using Scripts.Entities;
using UnityEngine;

namespace Scripts.Core
{
	public class Ladder : Ability
	{
		[field: SerializeField] public Transform TopMountPoint { get; private set; }
		[field: SerializeField] public Transform TopDismountPoint { get; private set; }
		[field: SerializeField] public Transform BottomMountPoint { get; private set; }
		[field: SerializeField] public Transform BottomDismountPoint { get; private set; }
		[field: SerializeField] public Transform RotationTransform { get; private set; }
	}
}