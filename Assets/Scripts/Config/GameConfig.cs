using Scripts.Config.Player;
using UnityEngine;

namespace Scripts.Config
{
	[CreateAssetMenu(menuName = "Config/" + nameof(GameConfig))]
	public class GameConfig : ScriptableObject
	{
		[field: SerializeField] public PlayerConfig Player { get; private set; }
		[field: SerializeField] public AudioConfig Audio { get; private set; }
	}
}