using System;
using DG.Tweening;
using UnityEngine;

namespace Scripts.Configs.Player
{
	[Serializable]
	public struct RandomShakeProcessorConfig
	{
		[field: SerializeField] public Vector3 PositionAmplitude { get; set; }
		[field: SerializeField] public float PositionFrequency { get; set; }
		[field: SerializeField] public Vector3 RotationAmplitude { get; set; }
		[field: SerializeField] public float RotationFrequency { get; set; }
		[field: SerializeField] public float ChangeDuration { get; set; }
		[field: SerializeField] public Ease ChangeEase { get; set; }

		public static readonly RandomShakeProcessorConfig Default = new();
	}
}