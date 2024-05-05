using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Scripts.Utility
{
	public abstract class ScriptableDictionary<TKey, TValue> : ScriptableObject
	{
		[SerializeField] protected TValue[] _configs;

		private Dictionary<TKey, TValue> _dictionary;

		private Dictionary<TKey, TValue> Dictionary =>
			_dictionary ??= _configs.ToDictionary(GetKey);

		protected abstract TKey GetKey(TValue value);

		public TValue this[TKey key] => Dictionary[key];

		public bool TryGet(TKey key, out TValue value)
		{
			return Dictionary.TryGetValue(key, out value);
		}
	}
}