using Zenject;

namespace Scripts.Utility.Extensions
{
	public static class DiContainerExtensions
	{
		public static T Instantiate<T>(this DiContainer diContainer, params object[] extraArgs)
		{
			return diContainer.Instantiate<T>(extraArgs);
		}
	}
}