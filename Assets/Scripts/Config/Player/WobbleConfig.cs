using System;
using Scripts.Core.Player.Shake;
using UnityEngine;

namespace Scripts.Config.Player
{
	[Serializable]
	public class WobbleConfig
	{
		[field: SerializeField] public float ChangeValuesDuration { get; private set; }
		[field: SerializeField] public ShakerConfig ShakerConfig { get; private set; }
		[field: SerializeField] public RandomShakeProcessorConfig StandIdleShakeConfig { get; private set; }
		[field: SerializeField] public RandomShakeProcessorConfig StandWalkShakeConfig { get; private set; }
		[field: SerializeField] public RandomShakeProcessorConfig StandRunShakeConfig { get; private set; }
		[field: SerializeField] public RandomShakeProcessorConfig CrouchIdleShakeConfig { get; private set; }
		[field: SerializeField] public RandomShakeProcessorConfig CrouchWalkShakeConfig { get; private set; }
	}
}