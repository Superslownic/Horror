using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Scripts
{
	[Serializable]
	public struct Uid
	{
		public string Value;

		public static implicit operator string(Uid id) => id.Value;

		public void Generate()
		{
			Value = Guid.NewGuid().ToString();
		}
	}
}