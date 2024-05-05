using Scripts.Units;

namespace Scripts.FSM.Composite
{
	public abstract class StateMachineAbility : Ability
	{
		public SuperState Root { get; protected set; }
		
		public State CurrentState
		{
			get
			{
				State state = Root;

				while (state is SuperState superState)
				{
					state = superState.CurrentState;
				}

				return state;
			}
		}
		
		protected override void OnInitialize()
		{
			Setup();
		}

		protected override void OnActivate()
		{
			Root.Enter();
		}

		protected override void OnUpdate()
		{
			Root.Update();
		}

		protected override void OnDeactivate()
		{
			Root.Exit();
		}

		protected abstract void Setup();

		public bool CurrentStateIs<T>() where T : State
		{
			State state = Root;

			while (state != null)
			{
				switch (state)
				{
					case T:
					{
						return true;
					}

					case SuperState superState:
					{
						state = superState.CurrentState;
						break;
					}

					default:
					{
						state = null;
						break;
					}
				}
			}

			return false;
		}
	}
}