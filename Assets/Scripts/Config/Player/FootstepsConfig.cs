using System;
using Scripts.Core.Player;
using UnityEngine;

namespace Scripts.Config.Player
{
	[Serializable]
	public class FootstepsConfig
	{
		[field: SerializeField] public float ChangeValuesDuration { get; private set; }
		[field: SerializeField] public ShakerConfig ShakerConfig { get; private set; }
		[field: SerializeField] public BobbingShakeProcessorConfig WalkShakeConfig { get; private set; }
		[field: SerializeField] public BobbingShakeProcessorConfig RunShakeConfig { get; private set; }
		[field: SerializeField] public BobbingShakeProcessorConfig CrouchShakeConfig { get; private set; }
	}
}