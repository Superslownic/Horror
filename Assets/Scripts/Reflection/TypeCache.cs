using System;

namespace Scripts.Reflection
{
	public static class TypeCache<T>
	{
		public static readonly Type Value = typeof(T);
	}
}