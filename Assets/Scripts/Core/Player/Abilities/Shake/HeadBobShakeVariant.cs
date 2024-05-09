using System;
using Scripts.Configs.Player;
using UnityEngine;

namespace Scripts.Core.Player
{
	[Serializable]
	public class HeadBobShakeVariant : IShakeVariant
	{
		[field: SerializeField] public TweenableVector3 PositionAmplitude { get; set; } = new();
		[field: SerializeField] public TweenableFloat PositionFrequency { get; set; } = new();
		[field: SerializeField] public TweenableVector3 RotationAmplitude { get; set; } = new();
		[field: SerializeField] public TweenableFloat RotationFrequency { get; set; } = new();

		public float Magnitude { get; set; }
		
		public float PositionTime { get; private set; }
		public float RotationTime { get; private set; }
		
		public void Update(out Vector3 position, out Vector3 rotation)
		{
			position = Vector3.zero;
			rotation = Vector3.zero;

			PositionTime += Time.deltaTime * Magnitude * PositionFrequency;
			RotationTime += Time.deltaTime * Magnitude * RotationFrequency;
			
			position.x = Mathf.Cos((PositionTime * 0.5f + 0.5f) * (Mathf.PI * 2)) * PositionAmplitude.x;
			position.y = Mathf.Cos((PositionTime + 0.5f) * (Mathf.PI * 2)) * PositionAmplitude.y;
			
			rotation.y = Mathf.Cos((RotationTime * 0.5f + 0.5f) * (Mathf.PI * 2)) * RotationAmplitude.x;
			rotation.x = Mathf.Cos((RotationTime + 0.5f) * (Mathf.PI * 2)) * RotationAmplitude.y;
		}

		public void Reset()
		{
			Magnitude = 0;
			PositionTime = 0;
			RotationTime = 0;
		}
	}
}