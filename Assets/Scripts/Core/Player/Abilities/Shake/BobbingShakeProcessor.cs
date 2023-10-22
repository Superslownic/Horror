using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Scripts.Core.Player.Shake
{
	[Serializable]
	public class BobbingShakeProcessor : IShakeProcessor
	{
		[field: SerializeField] public Vector3 PositionAmplitude { get; set; }
		[field: SerializeField] public float PositionFrequency { get; set; }
		[field: SerializeField] public Vector3 RotationAmplitude { get; set; }
		[field: SerializeField] public float RotationFrequency { get; set; }

		public float Magnitude { get; set; }
		
		[ShowInInspector] public float PositionTime { get; private set; }
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

		public IShakeProcessor Clone()
		{
			return new BobbingShakeProcessor
			{
				PositionAmplitude = PositionAmplitude,
				PositionFrequency = PositionFrequency,
				RotationAmplitude = RotationAmplitude,
				RotationFrequency = RotationFrequency
			};
		}
	}
}