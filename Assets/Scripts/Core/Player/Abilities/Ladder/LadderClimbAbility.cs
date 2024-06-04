using DG.Tweening;
using Scripts.Configs;
using Scripts.Configs.Player;
using Scripts.Input;
using Scripts.Reactive;
using Scripts.Units;
using Scripts.Utility.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace Scripts.Core.Player
{
	public class LadderClimbAbility : Ability
	{
		public DisposableAction MountAction { get; } = new();
		public DisposableAction DismountAction { get; } = new();

		[ShowInInspector] public bool IsClimbing { get; private set; }
		[ShowInInspector] public bool IsDismounting { get; private set; }
		[ShowInInspector] public LadderMarkerAbility Ladder { get; private set; }

		[SerializeField] private TriggerLink _triggerLink;

		[Inject] private readonly InputManager _inputManager;
		[Inject] private readonly GameConfig _gameConfig;

		private PlayerHeadAbility _playerHeadAbility;
		private PlayerBodyAbility _playerBodyAbility;
		private RigidbodyAbility _rigidbodyAbility;
		private AttachHeadAbility _attachHeadAbility;
		private GroundMoveAbility _groundMoveAbility;
		private LookAbility _lookAbility;
		private HeadBobAbility _headBobAbility;
		private LeanAbility _leanAbility;
		private ChangeVelocityAbility _changeVelocityAbility;
		private LadderValuesConfig _values;
		private float _smoothSpeed;
		private Vector3 _targetPosition;
		private float _dismountTime;
		private float _dismountTimer;
		private bool _canDismount;

		protected override void OnInitialize()
		{
			_playerHeadAbility = Unit.GetAbility<PlayerHeadAbility>();
			_playerBodyAbility = Unit.GetAbility<PlayerBodyAbility>();
			_rigidbodyAbility = Unit.GetAbility<RigidbodyAbility>();
			_attachHeadAbility = Unit.GetAbility<AttachHeadAbility>();
			_groundMoveAbility = Unit.GetAbility<GroundMoveAbility>();
			_lookAbility = Unit.GetAbility<LookAbility>();
			_headBobAbility = Unit.GetAbility<HeadBobAbility>();
			_leanAbility = Unit.GetAbility<LeanAbility>();
			_changeVelocityAbility = Unit.GetAbility<ChangeVelocityAbility>();

			_triggerLink.OnEnter.AddListener(HandleTriggerEnter).AddTo(Disposable);
			_triggerLink.OnExit.AddListener(HandleTriggerExit).AddTo(Disposable);
		}

		private void HandleTriggerEnter(Unit unit)
		{
			if(IsClimbing)
				return;

			if (unit.TryGetAbility(out LadderMarkerAbility ladder))
				Ladder = ladder;
		}

		private void HandleTriggerExit(Unit unit)
		{
			if(IsClimbing)
				return;

			if (unit.TryGetAbility(out LadderMarkerAbility ladder) && Ladder == ladder)
				Ladder = null;
		}

		protected override void OnUpdate()
		{
			if (!IsClimbing)
				return;

			float verticalInput = _inputManager.Move.ReadValue<Vector2>().y;
			Vector3 direction = Ladder.TopMountPoint.position - Ladder.BottomMountPoint.position;
			Vector3 closestPoint = Vector3Extensions.ClosestPointOnLine(_playerHeadAbility.HeadDetachedAnchor.position, Ladder.BottomMountPoint.position, Ladder.TopMountPoint.position);
			Vector3 stabDir = closestPoint - _playerHeadAbility.HeadDetachedAnchor.position;
			_changeVelocityAbility.SetTargetVelocity(direction.normalized * (verticalInput * _values.ClimbSpeed) + stabDir * 5);
			_rigidbodyAbility.Rigidbody.angularVelocity = Vector3.zero;

			if(_canDismount)
			{
				if (Vector3.Distance(Ladder.BottomMountPoint.position, _playerHeadAbility.HeadDetachedAnchor.position) >
				    Vector3.Distance(Ladder.BottomMountPoint.position, Ladder.TopMountPoint.position))
				{
					Dismount(Ladder.TopDismountPoint.position);
				}
				else if (Vector3.Distance(Ladder.TopMountPoint.position, _playerHeadAbility.HeadDetachedAnchor.position) >
				         Vector3.Distance(Ladder.BottomMountPoint.position.AddY(_gameConfig.Player.ChangeHeight.StandConfig.HeadHeight), Ladder.TopMountPoint.position))
				{
					Dismount(Ladder.BottomDismountPoint.position);
				}
			}
		}

		public void Mount()
		{
			_values = _gameConfig.Player.Ladder.DefaultValues;

			_groundMoveAbility.AddDeactivator(this);
			Unit.GetAbility<JumpAbility>().AddDeactivator(this);
			_lookAbility.AddDeactivator(this);
			_attachHeadAbility.AddDeactivator(this);
			_headBobAbility.ReplaceConfig(_values.ShakeConfig);
			_leanAbility.AddDeactivator(this);
			_changeVelocityAbility.AffectGravity = true;
			_changeVelocityAbility.SetActualVelocity(Vector3.zero);
			Unit.GetAbility<ChangeHeightAbility>().Execute(_gameConfig.Player.ChangeHeight.SwimConfig);

			_smoothSpeed = 0;
			IsClimbing = true;
			_canDismount = false;
			_targetPosition = Vector3Extensions.ClosestPointOnLine(_playerHeadAbility.HeadDetachedAnchor.position, Ladder.BottomMountPoint.position, Ladder.TopMountPoint.position);
			
			DOTween.To(() => _smoothSpeed, value => _smoothSpeed = value, _values.MaxSmoothDelta, _values.SmoothDeltaChangeTime).SetEase(Ease.InFlash);

			float distance = Vector3.Distance(_targetPosition, _playerHeadAbility.HeadDetachedAnchor.position);

			_rigidbodyAbility.Rigidbody.interpolation = RigidbodyInterpolation.None;
			DOTween.Sequence()
				.Append(_playerBodyAbility.BodyTransform.DOMoveX(_targetPosition.x, distance * _values.MountTimeMultiplier).SetEase(Ease.InOutFlash))
				.Join(_playerBodyAbility.BodyTransform.DOMoveZ(_targetPosition.z, distance * _values.MountTimeMultiplier).SetEase(Ease.InOutFlash))
				.Join(_playerBodyAbility.BodyTransform.DOMoveY(_targetPosition.y - _gameConfig.Player.ChangeHeight.StandConfig.HeadHeight, distance * _values.MountTimeMultiplier).SetEase(Ease.InOutFlash))
				.Join(_playerHeadAbility.HeadFloatingAnchor.DORotateQuaternion(Quaternion.LookRotation(Ladder.RotationTransform.forward), distance * _values.MountTimeMultiplier).SetEase(Ease.InOutCubic))
				.AppendCallback(() =>
				{
					_lookAbility.RemoveDeactivator(this);
					_canDismount = true;
					_rigidbodyAbility.Rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
				});

			MountAction.Invoke();
		}

		public void Dismount()
		{
			_changeVelocityAbility.AffectGravity = false;
			_groundMoveAbility.RemoveDeactivator(this);
			Unit.GetAbility<JumpAbility>().RemoveDeactivator(this);
			_leanAbility.RemoveDeactivator(this);
			IsClimbing = false;
			Ladder = null;
			DismountAction.Invoke();
		}

		private void Dismount(Vector3 targetPosition)
		{
			_playerHeadAbility.HeadDetachedAnchor.SetParent(null);
			_rigidbodyAbility.Rigidbody.gameObject.SetActive(false);
			Unit.GetAbility<ChangeHeightAbility>().Execute(_gameConfig.Player.ChangeHeight.StandConfig);
			_playerBodyAbility.BodyTransform.position = targetPosition;
			_rigidbodyAbility.Rigidbody.gameObject.SetActive(true);

			float distance = Vector3.Distance(_playerHeadAbility.HeadStaticAnchor.position, _playerHeadAbility.HeadDetachedAnchor.position);
			Unit.GetAbility<ConnectHeadToBodyAbility>().Execute(distance * _values.DismountTimeMultiplier);

			Dismount();
		}
	}
}