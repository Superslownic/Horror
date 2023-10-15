using Scripts.Config.Camera;
using Scripts.Config.Player;
using UnityEngine;

namespace Scripts.Config
{
	[CreateAssetMenu(menuName = "Configs/" + nameof(GameConfig))]
	public class GameConfig : ScriptableObject
	{
		[field: SerializeField] public PlayerConfig Player { get; private set; }
		[field: SerializeField] public CameraConfig Camera { get; private set; }
		[field: SerializeField] public AudioConfig Audio { get; private set; }
	}
}