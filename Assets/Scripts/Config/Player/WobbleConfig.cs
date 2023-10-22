using System;
using UnityEngine;

namespace Scripts.Config.Player
{
	[Serializable]
	public class WobbleConfig
	{
		[field: SerializeField] public float ChangeValuesDuration { get; private set; }
		[field: SerializeField] public RandomShakeProcessorValues StandingIdleValues { get; private set; }
		[field: SerializeField] public RandomShakeProcessorValues StandingWalkValues { get; private set; }
		[field: SerializeField] public RandomShakeProcessorValues StandingRunValues { get; private set; }
		[field: SerializeField] public RandomShakeProcessorValues CrouchIdleValues { get; private set; }
		[field: SerializeField] public RandomShakeProcessorValues CrouchWalkValues { get; private set; }
	}
}