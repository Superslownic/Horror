using Scripts.Utility;

namespace Scripts.Entities
{
	public static class ComponentExtensions
	{
		public static void AddActivator(this IActivatableAbility activatableAbility, object activator)
		{
			activatableAbility.gameObject.GetOrAddComponent<Activator>().AddActivator(activator);
		}
		
		public static void RemoveActivator(this IActivatableAbility activatableAbility, object activator)
		{
			activatableAbility.gameObject.GetOrAddComponent<Activator>().RemoveActivator(activator);
		}
		
		public static void AddDeactivator(this IDeactivatableAbility deactivatableAbility, object deactivator)
		{
			deactivatableAbility.gameObject.GetOrAddComponent<Deactivator>().AddDeactivator(deactivator);
		}
		
		public static void RemoveDeactivator(this IDeactivatableAbility deactivatableAbility, object deactivator)
		{
			deactivatableAbility.gameObject.GetOrAddComponent<Deactivator>().RemoveDeactivator(deactivator);
		}
	}
}