using System;
using Scripts.Core.Player.Shake;
using UnityEngine;

namespace Scripts.Config.Player
{
	[Serializable]
	public class CrouchConfig
	{
		[field: SerializeField] public float Duration { get; private set; }
		[field: SerializeField] public float TargetHeight { get; private set; }
		[field: SerializeField] public float TargetCameraHeight { get; private set; }
		[field: SerializeField] public ShakerConfig ShakeConfig { get; private set; }
		[field: SerializeField] public RandomShakeProcessorConfig PerformShakeProcessorConfig { get; private set; }
	}
}