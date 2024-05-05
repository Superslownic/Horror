using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI
{
	public class CursorView : MonoBehaviour
	{
		[field: SerializeField] public Image OutlineImage { get; private set; }
		[field: SerializeField] public Image CenterImage { get; private set; }
	}
}