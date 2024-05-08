using Scripts.Units;

namespace Scripts.Core.Player
{
	public class ApplyGravityAbility : Ability
	{
		private RigidbodyAbility _rigidbodyAbility;

		protected override void OnInitialize()
		{
			base.OnInitialize();
			_rigidbodyAbility = Unit.GetAbility<RigidbodyAbility>();
		}

		protected override void OnActivate()
		{
			base.OnActivate();
			_rigidbodyAbility.Rigidbody.useGravity = true;
		}

		protected override void OnDeactivate()
		{
			base.OnDeactivate();
			_rigidbodyAbility.Rigidbody.useGravity = false;
		}
	}
}