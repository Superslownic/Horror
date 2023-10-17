using System;
using UnityEngine;

namespace Scripts.Config.Player
{
	[Serializable]
	public class LookConfig
	{
		[field: SerializeField] public PlayerLookGamepadConfig Gamepad { get; private set; }
		[field: SerializeField] public PlayerLookKeyboardConfig Keyboard { get; private set; }
	}
}