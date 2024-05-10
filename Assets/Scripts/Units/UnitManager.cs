using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Units
{
	[DefaultExecutionOrder(-3)]
	public class UnitManager : MonoBehaviour
	{
		public static UnitManager Instance { get; private set; }

		public HashSet<Unit> UnitList { get; } = new();

		private void Awake()
		{
			Instance = this;
		}
	}
}