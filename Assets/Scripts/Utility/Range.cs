using System;
using UnityEngine;

namespace Scripts.Utility
{
	[Serializable]
	public class Range<T>
	{
		[field: SerializeField] public T Min { get; private set; }
		[field: SerializeField] public T Max { get; private set; }
	}
}