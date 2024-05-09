using System;
using Scripts.Core.Player;
using UnityEngine;

namespace Scripts.Configs.Player
{
	[Serializable]
	public class CrouchConfig
	{
		[field: SerializeField] public ShakerConfig ShakeConfig { get; private set; }
		[field: SerializeField] public RandomShakeProcessorConfig PerformShakeProcessorConfig { get; private set; }
	}
}