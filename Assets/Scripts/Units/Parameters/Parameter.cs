using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Scripts.Units.Parameters
{
	public class Parameter : MonoBehaviour, IModifiable
	{
		[field: SerializeField] public ParameterConfig Config { get; private set; }
		[field: SerializeField] public float BaseValue { get; private set; }

		[ShowInInspector] public float Value => _modifiers.Aggregate(BaseValue, (value, modifier) => modifier.Modify(value));

		private List<IModifier> _modifiers = new();

		public static implicit operator float(Parameter parameter) => parameter.Value;

		public void AddModifier(IModifier modifier)
		{
			_modifiers.Add(modifier);
		}

		public void RemoveModifier(IModifier modifier)
		{
			_modifiers.Remove(modifier);
		}
	}

	public interface IModifiable
	{
		void AddModifier(IModifier modifier);
		void RemoveModifier(IModifier modifier);
	}

	public interface IModifier
	{
		float Modify(float value);
	}
}