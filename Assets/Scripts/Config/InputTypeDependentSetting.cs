using System;
using Scripts.Input;
using Scripts.Utility;
using UnityEngine;

namespace Scripts.Config
{
	[Serializable]
	public class InputTypeDependentSetting<TValue>
	{
		[SerializeField] private SerializableDictionary<InputType, TValue> _dictionary;

		public TValue GetValue(InputType inputType)
		{
			return _dictionary[inputType];
		}
	}
}