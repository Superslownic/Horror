using System;
using UnityEngine;

namespace Scripts.Configs.Player
{
	[Serializable]
	public class LookConfig
	{
		[field: SerializeField] public float VerticalMinAngle { get; private set; }
		[field: SerializeField] public float VerticalMaxAngle { get; private set; }
		[field: SerializeField] public float PositionInterpolationSpeed { get; private set; }
		[field: SerializeField] public float RotationInterpolationSpeed { get; private set; }
		[field: SerializeField] public PlayerLookGamepadConfig Gamepad { get; private set; }
		[field: SerializeField] public PlayerLookKeyboardConfig Keyboard { get; private set; }
	}
}