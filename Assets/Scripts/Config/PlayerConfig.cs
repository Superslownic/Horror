using System;
using UnityEngine;

namespace Scripts.Config
{
	[Serializable]
	public class PlayerConfig
	{
		[field: SerializeField] public PlayerMovementConfig Movement { get; private set; }
		[field: SerializeField] public PlayerLookConfig Look { get; private set; }
	}
}