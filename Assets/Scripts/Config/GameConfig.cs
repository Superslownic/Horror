using UnityEngine;

namespace Scripts.Config
{
	[CreateAssetMenu(menuName = "Configs/" + nameof(GameConfig))]
	public class GameConfig : ScriptableObject
	{
		[field: SerializeField] public PlayerConfig Player { get; private set; }
	}
}