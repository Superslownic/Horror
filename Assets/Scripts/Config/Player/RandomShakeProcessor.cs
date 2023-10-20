using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Scripts.Config.Player
{
	public interface IShakeProcessor
	{
		void Update(out Vector3 position, out Vector3 rotation);
	}
	
	public enum ShakeProcessorType
	{
		Finite,
		Infinite
	}
	
	public enum ShakeProcessorState
	{
		Starting,
		Started,
		Stopping,
		Stopped
	}

	[Serializable]
	public class Shaker
	{
		[SerializeField, EnumToggleButtons, HideLabel]
		private ShakeProcessorType _type;
		
		[SerializeField, BoxGroup("Start", showLabel: false), HorizontalGroup("Start/Values"), LabelText("Start"), Min(0)]
		private float _startDuration;
		
		[SerializeField, BoxGroup("Start", showLabel: false), HorizontalGroup("Start/Values"), HideLabel]
		private AnimationCurve _startCurve;
		
		[SerializeField, HideIf("_type", ShakeProcessorType.Infinite), BoxGroup("Mid", showLabel: false), LabelText("Mid"), Min(0)]
		private float _midDuration;
		
		[SerializeField, BoxGroup("Stop", showLabel: false), HorizontalGroup("Stop/Values"), LabelText("Stop"), Min(0)]
		private float _stopDuration;
		
		[SerializeField, BoxGroup("Stop", showLabel: false), HorizontalGroup("Stop/Values"), HideLabel]
		private AnimationCurve _stopCurve;
		
		[SerializeReference]
		private IShakeProcessor _processor;

		[ShowInInspector, ReadOnly, FoldoutGroup("Debug"), PropertyOrder(1000)] private float _timer;
		[ShowInInspector, ReadOnly, FoldoutGroup("Debug"), PropertyOrder(1000)] private ShakeProcessorState _state;
		[ShowInInspector, ReadOnly, FoldoutGroup("Debug"), PropertyOrder(1000)] private float _stopTime;

		public void Update(out Vector3 position, out Vector3 rotation)
		{
			_timer += Time.deltaTime;

			float power = 0;
			
			switch (_state)
			{
				case ShakeProcessorState.Starting:
				{
					power = _startCurve.Evaluate(_timer / _startDuration);
					
					if (_timer > _startDuration)
					{
						_state = ShakeProcessorState.Started;
					}
					
					break;
				}

				case ShakeProcessorState.Started:
				{
					if (_type == ShakeProcessorType.Finite && _timer > _startDuration + _midDuration)
					{
						Stop();
					}
					
					power = 1;
					break;
				}

				case ShakeProcessorState.Stopping:
				{
					float stopTime = _timer - _stopTime;
					power = _stopCurve.Evaluate(stopTime / _stopDuration);
					
					if(stopTime >= _stopDuration)
					{
						_state = ShakeProcessorState.Stopped;
					}
					
					break;
				}

				case ShakeProcessorState.Stopped:
				{
					power = 0;
					break;
				}
			}
			
			_processor.Update(out Vector3 internalPosition, out Vector3 internalRotation);
			
			position = internalPosition * power;
			rotation = internalRotation * power;
		}

		[HorizontalGroup("Buttons"), Button(ButtonSizes.Small), GUIColor(0, 1, 0)]
		public void Start()
		{
			if (_state != ShakeProcessorState.Stopped)
				return;
			
			_state = ShakeProcessorState.Starting;
			_timer = 0;
			_stopTime = 0;
		}

		[HorizontalGroup("Buttons"), Button(ButtonSizes.Small), GUIColor(1, 0, 0)]
		public void Stop()
		{
			if (_state != ShakeProcessorState.Started)
				return;
			
			_state = ShakeProcessorState.Stopping;
			_stopTime = _timer < _startDuration ? _startDuration : _timer;
		}
	}

	[Serializable]
	public class RandomShakeProcessor : IShakeProcessor
	{
		[SerializeField] private float _horizontalAmplitude;
		[SerializeField] private float _verticalAmplitude;
		[SerializeField] private float _frequency;
		
		private bool _isInitialized;
		private Vector3 _rotationNoiseOffset;
		private Vector3 _rotationNoise;

		public void Update(out Vector3 position, out Vector3 rotation)
		{
			if (!_isInitialized)
			{
				_isInitialized = true;
				_rotationNoiseOffset.x = Random.Range(0f, 32f);
				_rotationNoiseOffset.y = Random.Range(0f, 32f);
				_rotationNoiseOffset.z = Random.Range(0f, 32f);
			}
			
			position = Vector3.zero;
			rotation = Vector3.zero;
			
			_rotationNoiseOffset += _frequency * Time.deltaTime * Vector3.one;

			_rotationNoise.x = Mathf.PerlinNoise(_rotationNoiseOffset.x, 0);
			_rotationNoise.y = Mathf.PerlinNoise(_rotationNoiseOffset.y, 1);
			_rotationNoise.z = Mathf.PerlinNoise(_rotationNoiseOffset.z, 2);
			
			_rotationNoise -= Vector3.one * 0.5f;

			rotation.x = _rotationNoise.x * _verticalAmplitude;
			rotation.y = _rotationNoise.y * _horizontalAmplitude;
			rotation.z = 0;
		}
	}
}