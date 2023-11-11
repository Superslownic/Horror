using System;
using UnityEngine;

namespace Scripts.Config.Player
{
	[Serializable]
	public class LadderValuesConfig
	{
		[field: SerializeField] public float ClimbSpeed { get; private set; }
		[field: SerializeField] public float MountTimeMultiplier { get; private set; }
		[field: SerializeField] public float DismountTimeMultiplier { get; private set; }
		[field: SerializeField] public float MaxSmoothDelta { get; private set; }
		[field: SerializeField] public float SmoothDeltaChangeTime { get; private set; }
		[field: SerializeField] public HeadBobShakeVaraintConfig ShakeConfig { get; private set; }
	}
}