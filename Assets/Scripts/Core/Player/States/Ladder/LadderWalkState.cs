using Scripts.TFSM;
using Scripts.Units;
using UnityEngine;
using Zenject;

namespace Scripts.Core.Player.States
{
	public class LadderWalkState : LeafState<LadderSuperState>
	{
		[Inject] private readonly Unit _playerUnit;

		public override void OnEnter()
		{
		}
	}
}