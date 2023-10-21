using System;
using UnityEngine;

namespace Scripts.Config.Player
{
	[Serializable]
	public class BobbingConfig
	{
		[field: SerializeField] public float ChangeDuration { get; private set; }
		[field: SerializeField] public float BreatheChangeDuration { get; private set; }
		[field: SerializeField] public BobbingValues IdleMinValues { get; private set; }
		[field: SerializeField] public BobbingValues IdleMaxValues { get; private set; }
		[field: SerializeField] public BobbingValues WalkingValues { get; private set; }
		[field: SerializeField] public BobbingValues RunningValues { get; private set; }
		[field: SerializeField] public BobbingValues CrouchingValues { get; private set; }
	}
}