using System.Linq;

namespace Scripts.Units
{
	public class OrCompositeActivator : CompositeActivator
	{
		public override bool State => _activators.Aggregate(false, (result, activator) => result | activator.State);
	}
}