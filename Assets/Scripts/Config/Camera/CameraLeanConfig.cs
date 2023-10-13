using System;
using UnityEngine;

namespace Scripts.Config.Camera
{
	[Serializable]
	public class CameraLeanConfig
	{
		[field: SerializeField] public float Threshold { get; private set; }
		[field: SerializeField] public float Angle { get; private set; }
		[field: SerializeField] public float Force { get; private set; }
		[field: SerializeField] public AnimationCurve Curve { get; private set; }
	}
}