using System;
using UnityEngine;

namespace Scripts.Config.Player
{
	[Serializable]
	public class SmoothConfig
	{
		[field: SerializeField] public float Force { get; private set; }
		[field: SerializeField] public float RotationForce { get; private set; }
	}
}