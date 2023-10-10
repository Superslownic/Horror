using Scripts.Entities;

namespace Scripts.FSM.Composite
{
	public abstract class StateMachineAbility : Ability
	{
		public SuperState Root { get; protected set; }
		
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
	}
}