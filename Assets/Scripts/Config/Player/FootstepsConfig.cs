using System;
using UnityEngine;

namespace Scripts.Config.Player
{
	[Serializable]
	public class FootstepsConfig
	{
		[field: SerializeField] public float ChangeValuesDuration { get; private set; }
		[field: SerializeField] public BobbingShakeProcessorValues WalkingValues { get; private set; }
		[field: SerializeField] public BobbingShakeProcessorValues RunningValues { get; private set; }
		[field: SerializeField] public BobbingShakeProcessorValues CrouchingValues { get; private set; }
	}
}