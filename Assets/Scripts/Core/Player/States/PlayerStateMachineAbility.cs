using System;
using Scripts.Reflection;
using Scripts.TFSM;

namespace Scripts.Core.Player.States
{
	public class PlayerStateMachineAbility : StateMachineAbility
	{
		protected override Type InitialState => TypeCache<StandIdleState>.Value;

		protected override void RegisterStates()
		{
			RegisterState<StandSuperState>();
			{
				RegisterState<StandIdleState>();
				{
					RegisterTransition<StandIdleToStandWalkTransition>();
					RegisterTransition<StandIdleToCrouchIdleTransition>();
					//RegisterTransition<StandIdleToLadderIdleTransition>();
					RegisterTransition<StandIdleToSwimIdleTransition>();
				}

				RegisterState<StandWalkState>();
				{
					RegisterTransition<StandWalkToStandIdleTransition>();
					RegisterTransition<StandWalkToStandRunTransition>();
					RegisterTransition<StandWalkToCrouchWalkTransition>();
					//RegisterTransition<StandWalkToLadderWalkTransition>();
					RegisterTransition<StandWalkToSwimWalkTransition>();
				}

				RegisterState<StandRunState>();
				{
					RegisterTransition<StandRunToStandIdleTransition>();
					RegisterTransition<StandRunToStandWalkTransition>();
					RegisterTransition<StandRunToCrouchWalkTransition>();
					//RegisterTransition<StandRunToLadderRunTransition>();
					RegisterTransition<StandRunToSwimWalkTransition>();
				}
			}

			RegisterState<CrouchSuperState>();
			{
				RegisterState<CrouchIdleState>();
				{
					RegisterTransition<CrouchIdleToCrouchWalkTransition>();
					RegisterTransition<CrouchIdleToStandIdleTransition>();
					//RegisterTransition<CrouchIdleToLadderIdleTransition>();
					RegisterTransition<CrouchIdleToSwimIdleTransition>();
				}

				RegisterState<CrouchWalkState>();
				{
					RegisterTransition<CrouchWalkToCrouchIdleTransition>();
					RegisterTransition<CrouchWalkToStandWalkTransition>();
					RegisterTransition<CrouchWalkToStandRunTransition>();
					//RegisterTransition<CrouchWalkToLadderWalkTransition>();
					RegisterTransition<CrouchWalkToSwimWalkTransition>();
				}
			}

			//RegisterState<LadderSuperState>();
			//RegisterState<LadderIdleState>();
			//RegisterState<LadderWalkState>();
			//RegisterState<LadderRunState>();

			RegisterState<SwimSuperState>();
			{
				RegisterState<SwimIdleState>();
				{
					RegisterTransition<SwimIdleToSwimWalkTransition>();
					RegisterTransition<SwimIdleToStandIdleTransition>();
				}

				RegisterState<SwimWalkState>();
				{
					RegisterTransition<SwimWalkToSwimIdleTransition>();
					RegisterTransition<SwimWalkToStandWalkTransition>();
				}
			}
		}

		private void Update()
		{
			StateMachine.Update();
		}
	}
}