using Scripts.Behaviour;
using Scripts.Reactive;
using Scripts.Units;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

namespace Scripts.Core
{
	public class CheckWaterAbility : Ability
	{
		public DisposableAction EnterAction { get; } = new();
		public DisposableAction ExitAction { get; } = new();

		[ShowInInspector] public bool IsInWater => _activationController.GetState();
		
		public WaterSurface WaterSurface { get; private set; }
		public float WaterSurfaceHeight { get; private set; }

		[SerializeField] private TriggerLink _trigger;
		[SerializeField] private Transform _anchor;

		private ActivationController _activationController = new();
		private CompositeDisposable _triggerDisposable = new();
		private WaterSearchParameters _searchParameters;
		private WaterSearchResult _searchResult;

		protected override void OnActivate()
		{
			base.OnActivate();
			_trigger.OnEnter.AddListener(HandleHeadTriggerEnter).AddTo(_triggerDisposable);
			_trigger.OnExit.AddListener(HandleHeadTriggerExit).AddTo(_triggerDisposable);
		}

		protected override void OnDeactivate()
		{
			base.OnDeactivate();
			_triggerDisposable.Dispose();
		}

		protected override void OnUpdate()
		{
			base.OnUpdate();
			FindWaterSurface();
		}

		private void FindWaterSurface()
		{
			_searchParameters.startPositionWS = _searchResult.candidateLocationWS;
			_searchParameters.targetPositionWS = _anchor.position;
			_searchParameters.error = 0.01f;
			_searchParameters.maxIterations = 8;

			if (WaterSurface != null && WaterSurface.ProjectPointOnWaterSurface(_searchParameters, out _searchResult))
			{
				WaterSurfaceHeight = _searchResult.projectedPositionWS.y;
			}
			else
			{
				WaterSurfaceHeight = float.MinValue;
			}
		}

		private void HandleHeadTriggerEnter(Unit unit)
		{
			if (unit.HasAbility<WaterMarkerAbility>())
			{
				WaterSurface = unit.GetAbility<WaterSurfaceAbility>().WaterSurface;
				_activationController.Add(unit);
				FindWaterSurface();
				EnterAction.Invoke();
			}
		}

		private void HandleHeadTriggerExit(Unit unit)
		{
			if (unit.HasAbility<WaterMarkerAbility>())
			{
				_activationController.Remove(unit);
				ExitAction.Invoke();
			}
		}
	}
}