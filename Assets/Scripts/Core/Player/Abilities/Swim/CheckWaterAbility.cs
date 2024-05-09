using Scripts.Reactive;
using Scripts.Units;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

namespace Scripts.Core.Player
{
	public class CheckWaterAbility : Ability
	{
		public bool InWater => IsActive && !_canStand;
		public WaterSurface WaterSurface { get; private set; }
		public float WaterSurfaceY { get; private set; }

		[SerializeField] private TriggerProvider _trigger;
		[SerializeField] private LayerMask _layerMask;

		private PlayerHeadAbility _playerHeadAbility;
		private WaterSearchParameters _searchParameters;
		private WaterSearchResult _searchResult;
		private bool _canStand;

		protected override void OnInitialize()
		{
			base.OnInitialize();
			_playerHeadAbility = Unit.GetAbility<PlayerHeadAbility>();
			_trigger.OnEnter.AddListener(HandleHeadTriggerEnter).AddTo(Disposable);
			_trigger.OnExit.AddListener(HandleHeadTriggerExit).AddTo(Disposable);
		}

		protected override void OnUpdate()
		{
			base.OnUpdate();

			_searchParameters.startPositionWS = _searchResult.candidateLocationWS;
			_searchParameters.targetPositionWS = _playerHeadAbility.HeadStaticAnchor.position;
			_searchParameters.error = 0.01f;
			_searchParameters.maxIterations = 8;

			if (WaterSurface != null && WaterSurface.ProjectPointOnWaterSurface(_searchParameters, out _searchResult))
			{
				WaterSurfaceY = _searchResult.projectedPositionWS.y;
			}
			else
			{
				WaterSurfaceY = float.MinValue;
			}

			bool isHit = Physics.SphereCast(
				origin: _playerHeadAbility.HeadStaticAnchor.position,
				radius: 0.3f,
				direction: Vector3.down,
				hitInfo: out RaycastHit hitInfo,
				maxDistance: 1000f,
				layerMask: _layerMask,
				queryTriggerInteraction: QueryTriggerInteraction.Ignore);

			if (!isHit)
			{
				_canStand = false;
				return;
			}

			float distanceToSurface = WaterSurfaceY - hitInfo.point.y;
			_canStand = IsActive && distanceToSurface <= 1.7f;
		}

		private void HandleHeadTriggerEnter(Unit unit)
		{
			if (unit.HasAbility<WaterMarkerAbility>())
			{
				WaterSurface = unit.GetAbility<WaterSurfaceAbility>().WaterSurface;
				AddActivator(unit);
			}
		}

		private void HandleHeadTriggerExit(Unit unit)
		{
			if (unit.HasAbility<WaterMarkerAbility>())
			{
				RemoveActivator(unit);
			}
		}
	}
}