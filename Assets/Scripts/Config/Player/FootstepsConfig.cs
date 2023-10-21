using System;
using UnityEngine;

namespace Scripts.Config.Player
{
	[Serializable]
	public class FootstepsConfig
	{
		[field: SerializeField] public float ChangeValuesDuration { get; private set; }
		[field: SerializeField] public BobbingValues WalkingValues { get; private set; }
		[field: SerializeField] public BobbingValues RunningValues { get; private set; }
		[field: SerializeField] public BobbingValues CrouchingValues { get; private set; }
	}
}