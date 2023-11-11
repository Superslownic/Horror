using System;
using Scripts.Config.Player;
using UnityEngine;

namespace Scripts.Core.Player
{
	[Serializable]
	public class HeadBobShakeVariant : IShakeVariant
	{
		[field: SerializeField] public ReplaceableVector3 PositionAmplitude { get; set; } = new();
		[field: SerializeField] public ReplaceableFloat PositionFrequency { get; set; } = new();
		[field: SerializeField] public ReplaceableVector3 RotationAmplitude { get; set; } = new();
		[field: SerializeField] public ReplaceableFloat RotationFrequency { get; set; } = new();

		public float Magnitude { get; set; }
		
		public float PositionTime { get; private set; }
		public float RotationTime { get; private set; }
		
		public void Update(out Vector3 position, out Vector3 rotation)
		{
			position = Vector3.zero;
			rotation = Vector3.zero;

			PositionTime += Time.deltaTime * Magnitude * PositionFrequency.Value;
			RotationTime += Time.deltaTime * Magnitude * RotationFrequency.Value;
			
			position.x = Mathf.Cos((PositionTime * 0.5f + 0.5f) * (Mathf.PI * 2)) * PositionAmplitude.Value.x;
			position.y = Mathf.Cos((PositionTime + 0.5f) * (Mathf.PI * 2)) * PositionAmplitude.Value.y;
			
			rotation.y = Mathf.Cos((RotationTime * 0.5f + 0.5f) * (Mathf.PI * 2)) * RotationAmplitude.Value.x;
			rotation.x = Mathf.Cos((RotationTime + 0.5f) * (Mathf.PI * 2)) * RotationAmplitude.Value.y;
		}

		public void Reset()
		{
			Magnitude = 0;
			PositionTime = 0;
			RotationTime = 0;
		}
	}
}