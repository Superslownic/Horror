using Scripts.TFSM;
using Scripts.Units;
using Zenject;

namespace Scripts.Core.Player.States
{
	public class LadderRunState : LeafState
	{
		[Inject] private readonly Unit _playerUnit;
	}
}