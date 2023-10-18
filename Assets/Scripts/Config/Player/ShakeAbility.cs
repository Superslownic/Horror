using Scripts.Entities;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Scripts.Config.Player
{
	public class ShakeAbility : Ability
	{
		[SerializeField] private Transform _anchor;
		[SerializeField] private float _horizontalAmplitude;
		[SerializeField] private float _verticalAmplitude;
		[SerializeField] private AnimationCurve _smoothCurve;
		[SerializeField] private float _frequency;
		[SerializeField] private float _time;
		
		private float _timer;
		private Vector3 rotationNoiseOffset;
		private Vector3 rotationNoise;

		[Button]
		public void Shake()
		{
			_timer = _time;
			rotationNoiseOffset.x = Random.Range(0f, 32f);
			rotationNoiseOffset.y = Random.Range(0f, 32f);
			rotationNoiseOffset.z = Random.Range(0f, 32f);
		}

		protected override void OnUpdate()
		{
			if (_timer < 0)
			{
				return;
			}
			
			_timer -= Time.deltaTime;
			
			rotationNoiseOffset += _frequency * Time.deltaTime * Vector3.one;

			rotationNoise.x = Mathf.PerlinNoise(rotationNoiseOffset.x, 0);
			rotationNoise.y = Mathf.PerlinNoise(rotationNoiseOffset.y, 1);
			rotationNoise.z = Mathf.PerlinNoise(rotationNoiseOffset.z, 2);
			
			rotationNoise -= Vector3.one * 0.5f;
			
			float t = _smoothCurve.Evaluate(1 - _timer / _time);
			
			_anchor.localRotation = Quaternion.Euler
			(
				rotationNoise.x * t * _verticalAmplitude,
				rotationNoise.y * t * _horizontalAmplitude,
				0
			);
		}
	}
}