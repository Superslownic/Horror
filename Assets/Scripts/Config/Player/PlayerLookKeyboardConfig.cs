using System;
using UnityEngine;

namespace Scripts.Config.Player
{
	[Serializable]
	public class PlayerLookKeyboardConfig
	{
		[field: SerializeField] public float Sensitivity { get; private set; }
	}
}