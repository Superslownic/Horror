using System;
using UnityEngine;

namespace Scripts.Config
{
	[Serializable]
	public class PlayerLookConfig
	{
		[field: SerializeField] public float Sensitivity { get; private set; }
		[field: SerializeField] public float Acceleration { get; private set; }
		[field: SerializeField] public float Deceleration { get; private set; }
	}
}