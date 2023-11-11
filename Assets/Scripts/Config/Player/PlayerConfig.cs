using System;
using UnityEngine;

namespace Scripts.Config.Player
{
	[Serializable]
	public class PlayerConfig
	{
		[field: SerializeField] public MovementConfig Movement { get; private set; }
		[field: SerializeField] public LookConfig Look { get; private set; }
		[field: SerializeField] public CrouchConfig Crouch { get; private set; }
		[field: SerializeField] public LeanConfig Lean { get; private set; }
		[field: SerializeField] public HeadBobConfig Footsteps { get; private set; }
		[field: SerializeField] public BreathConfig Breath { get; private set; }
		[field: SerializeField] public HeadSwayingConfig Wobble { get; private set; }
		[field: SerializeField] public FlashlightConfig Flashlight { get; private set; }
		[field: SerializeField] public LadderConfig Ladder { get; private set; }
	}
}