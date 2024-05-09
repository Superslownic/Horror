using System;
using DG.Tweening;
using UnityEngine;

namespace Scripts.Configs.Player
{
	public abstract class Tweenable<T>
	{
		[field: SerializeField] public T Value { get; set; }
		public Tween _tween { get; set; }

		public void Set(T value)
		{
			_tween?.Kill();
			Value = value;
		}

		public void Tween(T target, float time, Ease ease)
		{
			_tween?.Kill();
			_tween = CreateTween(target, time, ease);
		}
		
		public abstract Tween CreateTween(T target, float time, Ease ease);
	}

	[Serializable]
	public class TweenableFloat : Tweenable<float>
	{
		public override Tween CreateTween(float target, float time, Ease ease)
		{
			return DOTween.To(() => Value, value => Value = value, target, time).SetEase(ease);
		}

		public static implicit operator float(TweenableFloat tweenableFloat) => tweenableFloat.Value;
	}
	
	[Serializable]
	public class TweenableVector3 : Tweenable<Vector3>
	{
		public float x => Value.x;
		public float y => Value.y;
		public float z => Value.z;
		
		public override Tween CreateTween(Vector3 target, float time, Ease ease)
		{
			return DOTween.To(() => Value, value => Value = value, target, time).SetEase(ease);
		}
		
		public static implicit operator Vector3(TweenableVector3 tweenableVector3) => tweenableVector3.Value;
	}
}