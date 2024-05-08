using Scripts.FSM.Composite;
using Scripts.Units;
using UnityEngine;
using Zenject;

namespace Scripts.Core.Player.States
{
	public class StandingSuperState : SuperState
	{
		[Inject] private readonly Unit _playerUnit;

		public StandingSuperState(string name, State initialState) : base(name, initialState)
		{
		}

		public override void Enter()
		{
			_playerUnit.GetAbility<PlayerBodyAbility>().WalkCollider.enabled = true;
			_playerUnit.GetAbility<ChangeVelocityAbility>().AffectGravity = false;
			_playerUnit.GetAbility<GroundMoveAbility>().AddActivator(this);
			_playerUnit.GetAbility<HeadBobAbility>().AddActivator(this);
			base.Enter();
		}

		public override void Exit()
		{
			_playerUnit.GetAbility<PlayerBodyAbility>().WalkCollider.enabled = false;
			_playerUnit.GetAbility<HeadBobAbility>().RemoveActivator(this);
			_playerUnit.GetAbility<GroundMoveAbility>().RemoveActivator(this);
			base.Exit();
		}
	}
}