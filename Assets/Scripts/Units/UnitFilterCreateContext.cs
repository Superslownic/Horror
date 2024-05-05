using System;
using Scripts.Reactive;
using Scripts.Reflection;

namespace Scripts.Units
{
	public struct UnitFilterCreateContext
	{
		public Type[] Includes;
		public Type[] Excludes;

		public UnitFilterCreateContext With(params Type[] abilities)
		{
			Includes = abilities;
			return this;
		}

		public UnitFilterCreateContext Without(params Type[] abilities)
		{
			Excludes = abilities;
			return this;
		}

		public UnitFilter Build(IGroupedDisposable disposable)
		{
			UnitFilter filter = new UnitFilter(Includes, Excludes);
			disposable?.Add(filter);
			return filter;
		}

		public UnitFilterCreateContext With<T>() => With(TypeCache<T>.Value);
		public UnitFilterCreateContext With<T1, T2>() => With(TypeCache<T1>.Value, TypeCache<T2>.Value);
		public UnitFilterCreateContext With<T1, T2, T3>() => With(TypeCache<T1>.Value, TypeCache<T2>.Value, TypeCache<T3>.Value);
		public UnitFilterCreateContext With<T1, T2, T3, T4>() => With(TypeCache<T1>.Value, TypeCache<T2>.Value, TypeCache<T3>.Value, TypeCache<T4>.Value);
		public UnitFilterCreateContext With<T1, T2, T3, T4, T5>() => With(TypeCache<T1>.Value, TypeCache<T2>.Value, TypeCache<T3>.Value, TypeCache<T4>.Value, TypeCache<T5>.Value);

		public UnitFilterCreateContext Without<T>() => Without(TypeCache<T>.Value);
		public UnitFilterCreateContext Without<T1, T2>() => Without(TypeCache<T1>.Value, TypeCache<T2>.Value);
		public UnitFilterCreateContext Without<T1, T2, T3>() => Without(TypeCache<T1>.Value, TypeCache<T2>.Value, TypeCache<T3>.Value);
		public UnitFilterCreateContext Without<T1, T2, T3, T4>() => Without(TypeCache<T1>.Value, TypeCache<T2>.Value, TypeCache<T3>.Value, TypeCache<T4>.Value);
		public UnitFilterCreateContext Without<T1, T2, T3, T4, T5>() => Without(TypeCache<T1>.Value, TypeCache<T2>.Value, TypeCache<T3>.Value, TypeCache<T4>.Value, TypeCache<T5>.Value);
	}
}