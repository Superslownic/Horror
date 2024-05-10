using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Scripts.Reactive;
using Sirenix.OdinInspector;

namespace Scripts.Units
{
	public class UnitFilter : IDisposable, IEnumerable<Unit>
	{
		public DisposableAction ChangedAction { get; } = new();
		public DisposableAction<Unit> AddedAction { get; } = new();
		public DisposableAction<Unit> RemovedAction { get; } = new();

		public int Count => _units.Count;

		[ShowInInspector, ReadOnly] private Type[] _include;
		[ShowInInspector, ReadOnly] private Type[] _exclude;

		[ShowInInspector, ReadOnly] private HashSet<Unit> _units = new();
		private CompositeDisposable _disposable = new();

		public UnitFilter(Type[] include = null, Type[] exclude = null)
		{
			_include = include ?? Array.Empty<Type>();
			_exclude = exclude ?? Array.Empty<Type>();

			Unit.InitializedAction.AddListener(HandleUnitInitialized).AddTo(_disposable);
			Unit.DisposedAction.AddListener(HandleUnitDisposed).AddTo(_disposable);

			Ability.InitializedAction.AddListener(HandleAbilityInitialized);
			Ability.DisposedAction.AddListener(HandleAbilityDisposed);

			foreach (Unit unit in UnitManager.Instance.UnitList)
				ManageUnit(unit);
		}

		public static UnitFilterCreateContext Create()
		{
			return new UnitFilterCreateContext();
		}

		public void Dispose()
		{
			_disposable.Dispose();
		}

		public IEnumerator<Unit> GetEnumerator()
		{
			return _units.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		private void HandleUnitInitialized(Unit unit)
		{
			ManageUnit(unit);
		}

		private void HandleAbilityInitialized(Ability ability)
		{
			ManageUnit(ability.Unit);
		}

		private void HandleUnitDisposed(Unit unit)
		{
			if (_units.Remove(unit))
			{
				ChangedAction.Invoke();
				RemovedAction.Invoke(unit);
			}
		}

		private void HandleAbilityDisposed(Ability ability)
		{
			ManageUnit(ability.Unit);
		}

		private void ManageUnit(Unit unit)
		{
			if (!_exclude.Any(unit.HasAbility) && _include.All(unit.HasAbility))
			{
				if (_units.Add(unit))
				{
					ChangedAction.Invoke();
					AddedAction.Invoke(unit);
				}
				return;
			}

			if (_units.Remove(unit))
			{
				ChangedAction.Invoke();
				RemovedAction.Invoke(unit);
			}
		}
	}
}