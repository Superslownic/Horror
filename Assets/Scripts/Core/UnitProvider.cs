using Scripts.Entities;
using UnityEngine;

namespace Scripts.Core
{
	public class UnitProvider : MonoBehaviour
	{
		[field: SerializeField] public Unit Unit { get; private set; }
	}
}