using Scripts.Units;

namespace Scripts.Core
{
	public class Interactable : Ability
	{
		public Unit User { get; private set; }
		
		//private IActivatableAbility[] _abilities;

		protected override void OnInitialize()
		{
			//_abilities = GetComponentsInChildren<IActivatableAbility>();
		}

		public void Activate(Unit user)
		{
			User = user;
			
			/*foreach (IActivatableAbility ability in _abilities)
			{
				ability.AddActivator(this);
			}*/
		}

		public void Deactivate()
		{
			User = null;
			
			/*foreach (IActivatableAbility ability in _abilities)
			{
				ability.RemoveActivator(this);
			}*/
		}
	}
}