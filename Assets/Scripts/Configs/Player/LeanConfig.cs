using System;
using UnityEngine;

namespace Scripts.Configs.Player
{
	[Serializable]
	public class LeanConfig
	{
		[field: SerializeField] public float Angle { get; private set; }
		[field: SerializeField] public float Force { get; private set; }
	}
}