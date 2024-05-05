using Scripts.Units;
using UnityEngine;

namespace Scripts.Core
{
	public class UnitLink : MonoBehaviour
	{
		[field: SerializeField] public Unit Unit { get; private set; }
	}
}