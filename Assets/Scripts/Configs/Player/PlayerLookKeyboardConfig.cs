using System;
using UnityEngine;

namespace Scripts.Configs.Player
{
	[Serializable]
	public class PlayerLookKeyboardConfig
	{
		[field: SerializeField] public float Sensitivity { get; private set; }
	}
}