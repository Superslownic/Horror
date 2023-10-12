using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Scripts.Tweener
{
	/*public class TweenManager
	{
		public Sequence CreateSequence()
		{
			return DOTween.Sequence();
		}
	}

	public class Tweener : IContext
	{
		[SerializeReference] private ITween _processor;

		private Dictionary<string, IAccumulator> _accumulators = new();
		private Coroutine _coroutine;
		
		public void Start(MonoBehaviour target)
		{
			_coroutine = target.StartCoroutine(Routine());
		}

		public void Stop()
		{
			
		}

		public ProceduralAnimationResult Update()
		{
			return _processor.Execute(this);
		}

		private IEnumerator Routine()
		{
			
		}

		public bool TryGetAccumulator<T>(string key, out T accumulator) where T : IAccumulator
		{
			bool result = _accumulators.TryGetValue(key, out IAccumulator output);
			accumulator = (T)output;
			return result;
		}

		public void AddAccumulator(string key, IAccumulator accumulator)
		{
			_accumulators.Add(key, accumulator);
		}
	}

	public enum ProceduralAnimationResult
	{
		Pending,
		Complete
	}

	public interface ITween
	{
		ProceduralAnimationResult Execute(IContext context);
	}
	
	public interface IContext
	{
		bool TryGetAccumulator<T>(string key, out T accumulator) where T : IAccumulator;
		void AddAccumulator(string key, IAccumulator accumulator);
	}

	public interface IAccumulator
	{
		
	}

	public class TransformLocalPositionAccumulator : IAccumulator
	{
		private Transform _target;
		private Vector3 _startValue;
		private Vector3 _resultValue;

		public TransformLocalPositionAccumulator(Transform target)
		{
			_target = target;
			_startValue = _target.localPosition;
		}

		public void Reset()
		{
			_resultValue = _startValue;
		}

		public void Accumulate(Vector3 delta)
		{
			_resultValue += delta;
		}

		public void Apply()
		{
			_target.localPosition = _resultValue;
		}
	}

	[Serializable]
	public class TransformLocalPositionTween : ITween
	{
		[SerializeField] private Transform _target;
		[SerializeField] private Vector3 _axis;
		[SerializeField] private float _duration;
		
		private TransformLocalPositionAccumulator _accumulator;
		private float _timer;

		public TransformLocalPositionTween(Vector3 axis, float duration)
		{
			_axis = axis;
			_duration = duration;
		}

		public void Initialize(IContext context)
		{
			string key = _target.GetInstanceID().ToString();
			
			if (!context.TryGetAccumulator(key, out TransformLocalPositionAccumulator _accumulator))
			{
				_accumulator = new TransformLocalPositionAccumulator(_target);
				context.AddAccumulator(key, _accumulator);
			}
		}

		public ProceduralAnimationResult Execute(IContext context)
		{
			_timer += Time.deltaTime;
			_accumulator.Accumulate();
		}
	}*/
}