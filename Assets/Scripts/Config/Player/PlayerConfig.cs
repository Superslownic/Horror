using System;
using UnityEngine;

namespace Scripts.Config.Player
{
	[Serializable]
	public class PlayerConfig
	{
		[field: SerializeField] public MovementConfig Movement { get; private set; }
		[field: SerializeField] public LookConfig Look { get; private set; }
		[field: SerializeField] public LeanConfig Lean { get; private set; }
		[field: SerializeField] public SmoothConfig SmoothPosition { get; private set; }
		[field: SerializeField] public FootstepsConfig Footsteps { get; private set; }
		[field: SerializeField] public BreathConfig Breath { get; private set; }
	}
}