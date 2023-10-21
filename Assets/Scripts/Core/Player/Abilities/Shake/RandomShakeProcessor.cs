using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Scripts.Core.Player.Shake
{
	[Serializable]
	public class RandomShakeProcessor : IShakeProcessor
	{
		[field: SerializeField] public Vector3 PositionAmplitude { get; private set; }
		[field: SerializeField] public float PositionFrequency { get; private set; }
		[field: SerializeField] public Vector3 RotationAmplitude { get; private set; }
		[field: SerializeField] public float RotationFrequency { get; private set; }

		private bool _isInitialized;
		private Vector3 _positionNoiseOffset;
		private Vector3 _rotationNoiseOffset;
		private Vector3 _positionNoise;
		private Vector3 _rotationNoise;

		public void Update(out Vector3 position, out Vector3 rotation)
		{
			if (!_isInitialized)
			{
				_isInitialized = true;
				_positionNoiseOffset.x = Random.Range(0f, 32f);
				_positionNoiseOffset.y = Random.Range(0f, 32f);
				_positionNoiseOffset.z = Random.Range(0f, 32f);
				_rotationNoiseOffset.x = Random.Range(0f, 32f);
				_rotationNoiseOffset.y = Random.Range(0f, 32f);
				_rotationNoiseOffset.z = Random.Range(0f, 32f);
			}
			
			position = Vector3.zero;
			rotation = Vector3.zero;
			
			_positionNoiseOffset += PositionFrequency * Time.deltaTime * Vector3.one;
			
			_positionNoise.x = Mathf.PerlinNoise(_positionNoiseOffset.x, 0);
			_positionNoise.y = Mathf.PerlinNoise(_positionNoiseOffset.y, 1);
			_positionNoise.z = Mathf.PerlinNoise(_positionNoiseOffset.z, 2);
			
			_positionNoise -= Vector3.one * 0.5f;
			
			_rotationNoiseOffset += RotationFrequency * Time.deltaTime * Vector3.one;

			_rotationNoise.x = Mathf.PerlinNoise(_rotationNoiseOffset.x, 0);
			_rotationNoise.y = Mathf.PerlinNoise(_rotationNoiseOffset.y, 1);
			_rotationNoise.z = Mathf.PerlinNoise(_rotationNoiseOffset.z, 2);
			
			_rotationNoise -= Vector3.one * 0.5f;
			
			position.x = _positionNoise.x * PositionAmplitude.x;
			position.y = _positionNoise.y * PositionAmplitude.y;
			position.z = _positionNoise.z * PositionAmplitude.z;

			rotation.x = _rotationNoise.x * RotationAmplitude.x;
			rotation.y = _rotationNoise.y * RotationAmplitude.y;
			rotation.z = _rotationNoise.z * RotationAmplitude.z;
		}

		public IShakeProcessor Clone()
		{
			return new RandomShakeProcessor
			{
				PositionAmplitude = PositionAmplitude,
				PositionFrequency = PositionFrequency,
				RotationAmplitude = RotationAmplitude,
				RotationFrequency = RotationFrequency
			};
		}
	}
}