using System;
using Scripts.Config.Player;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Scripts.Core.Player.Shake
{
	[Serializable]
	public class RandomShakeProcessor : IShakeProcessor
	{
		[field: SerializeField] public ReplaceableVector3 PositionAmplitude { get; set; } = new();
		[field: SerializeField] public ReplaceableFloat PositionFrequency { get; set; } = new();
		[field: SerializeField] public ReplaceableVector3 RotationAmplitude { get; set; } = new();
		[field: SerializeField] public ReplaceableFloat RotationFrequency { get; set; } = new();

		private Vector3 _positionNoiseOffset;
		private Vector3 _rotationNoiseOffset;

		public void Reset()
		{
			_positionNoiseOffset.x = Random.Range(0f, 32f);
			_positionNoiseOffset.y = Random.Range(0f, 32f);
			_positionNoiseOffset.z = Random.Range(0f, 32f);
			_rotationNoiseOffset.x = Random.Range(0f, 32f);
			_rotationNoiseOffset.y = Random.Range(0f, 32f);
			_rotationNoiseOffset.z = Random.Range(0f, 32f);
		}

		public void Update(out Vector3 position, out Vector3 rotation)
		{
			position = Vector3.zero;
			rotation = Vector3.zero;
			
			_positionNoiseOffset += PositionFrequency.Value * Time.deltaTime * Vector3.one;

			Vector3 positionNoise = new Vector3
			{
				x = Mathf.PerlinNoise(_positionNoiseOffset.x, 0),
				y = Mathf.PerlinNoise(_positionNoiseOffset.y, 1),
				z = Mathf.PerlinNoise(_positionNoiseOffset.z, 2)
			};
			
			positionNoise -= Vector3.one * 0.5f;
			
			_rotationNoiseOffset += RotationFrequency.Value * Time.deltaTime * Vector3.one;
			
			Vector3 rotationNoise = new Vector3
			{
				x = Mathf.PerlinNoise(_rotationNoiseOffset.x, 0),
				y = Mathf.PerlinNoise(_rotationNoiseOffset.y, 1),
				z = Mathf.PerlinNoise(_rotationNoiseOffset.z, 2)
			};

			rotationNoise.x = Mathf.PerlinNoise(_rotationNoiseOffset.x, 0);
			rotationNoise.y = Mathf.PerlinNoise(_rotationNoiseOffset.y, 1);
			rotationNoise.z = Mathf.PerlinNoise(_rotationNoiseOffset.z, 2);
			
			rotationNoise -= Vector3.one * 0.5f;
			
			position.x = positionNoise.x * PositionAmplitude.Value.x;
			position.y = positionNoise.y * PositionAmplitude.Value.y;
			position.z = positionNoise.z * PositionAmplitude.Value.z;

			rotation.x = rotationNoise.x * RotationAmplitude.Value.x;
			rotation.y = rotationNoise.y * RotationAmplitude.Value.y;
			rotation.z = rotationNoise.z * RotationAmplitude.Value.z;
		}
	}
}