using Scripts.TFSM;
using Scripts.Units;
using Zenject;

namespace Scripts.Core.Player.States
{
	public class LadderSuperState : SuperState
	{
	}

	public class LadderIdleState : LeafState
	{
		[Inject] private readonly Unit _playerUnit;
	}

	public class LadderWalkState : LeafState
	{
		[Inject] private readonly Unit _playerUnit;
	}

	public class LadderRunState : LeafState
	{
		[Inject] private readonly Unit _playerUnit;
	}
}