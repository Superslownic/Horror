using Scripts.Audio;
using Scripts.Entities;
using Zenject;

namespace Scripts.Core.Player
{
	public class FootstepsAbility : Ability
	{
		[Inject] private readonly AudioManager _audioManager;
	}
}