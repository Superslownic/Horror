using UnityEngine;

namespace Scripts.Units
{
	public class UnitProvider : MonoBehaviour
	{
		[field: SerializeField] public Unit Unit { get; private set; }
	}
}