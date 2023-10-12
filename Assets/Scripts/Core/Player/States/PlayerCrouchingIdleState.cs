using Scripts.Core.Player.Movement;
using Scripts.Entities;
using Scripts.FSM.Composite;
using UnityEngine;

namespace Scripts.Core.Player.States
{
	public class PlayerCrouchingIdleState : State
	{
		public PlayerCrouchingIdleState(string name) : base(name)
		{
		}

		public override void Enter()
		{
			Object.FindObjectOfType<Entity>().GetAbility<PlayerCrouchAbility>().Crouch();
		}

		public override void Update()
		{
		}

		public override void Exit()
		{
			Object.FindObjectOfType<Entity>().GetAbility<PlayerCrouchAbility>().Stand();
		}
	}
}