//using Scripts.UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Scripts.Configs
{
	[CreateAssetMenu(menuName = CreateAssetMenuTabs.CONFIGS + CreateAssetMenuTabs.UI + nameof(UIConfig))]
	public class UIConfig : Config
	{
		[field: SerializeField] public Camera UICameraPrefab { get; private set; }
		[field: SerializeField] public EventSystem UIEventSystemPrefab { get; private set; }
		//[field: SerializeField] public UIRoot UIRootPrefab { get; private set; }
	}
}