using System.Linq;

namespace Scripts.Units
{
	public class AndCompositeActivator : CompositeActivator
	{
		public override bool State => _activators.Aggregate(true, (result, activator) => result & activator.State);
	}
}