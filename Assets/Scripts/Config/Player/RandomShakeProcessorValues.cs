using System;
using UnityEngine;

namespace Scripts.Config.Player
{
	[Serializable]
	public struct RandomShakeProcessorValues
	{
		[field: SerializeField] public Vector3 PositionAmplitude { get; set; }
		[field: SerializeField] public float PositionFrequency { get; set; }
		[field: SerializeField] public Vector3 RotationAmplitude { get; set; }
		[field: SerializeField] public float RotationFrequency { get; set; }

		public static readonly RandomShakeProcessorValues Default = new();
	}
}