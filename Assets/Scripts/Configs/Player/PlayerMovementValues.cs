using System;
using UnityEngine;

namespace Scripts.Configs.Player
{
	[Serializable]
	public class PlayerMovementValues
	{
		[field: SerializeField] public float Speed { get; private set; }
	}
}