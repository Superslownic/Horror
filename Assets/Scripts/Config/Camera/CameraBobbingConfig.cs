using System;
using UnityEngine;

namespace Scripts.Config.Camera
{
	[Serializable]
	public class CameraBobbingConfig
	{
		[field: SerializeField] public float Threshold { get; private set; }
		[field: SerializeField] public float ChangeDuration { get; private set; }
		[field: SerializeField] public CameraBobbingValues WalkingValues { get; private set; }
		[field: SerializeField] public CameraBobbingValues RunningValues { get; private set; }
		[field: SerializeField] public CameraBobbingValues CrouchingValues { get; private set; }
	}
}