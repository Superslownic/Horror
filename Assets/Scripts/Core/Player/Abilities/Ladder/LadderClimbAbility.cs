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
		[SerializeField] private float _acceleration;

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
		private LadderValuesConfig _values;
		private float _smoothSpeed;
		private Vector3 _targetPosition;
		private Vector3 _velocity;
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

		protected override void OnFixedUpdate()
		{
			if (!IsClimbing)
				return;

			float verticalInput = _inputManager.Move.ReadValue<Vector2>().y;
			Vector3 direction = Ladder.TopMountPoint.position - Ladder.BottomMountPoint.position;
			Vector3 closestPoint = Vector3Extensions.ClosestPointOnLine(_playerBodyAbility.BodyTransform.position, Ladder.BottomMountPoint.position, Ladder.TopMountPoint.position);
			Vector3 stabDir = closestPoint - _playerBodyAbility.BodyTransform.position;
			_velocity = Vector3.Lerp(_velocity, direction.normalized * (verticalInput * _values.ClimbSpeed) + stabDir * 5, _acceleration * Time.deltaTime);
			_rigidbodyAbility.Rigidbody.linearVelocity = _velocity;
			_rigidbodyAbility.Rigidbody.angularVelocity = Vector3.zero;

			if (!_canDismount)
				return;

			float t = Vector3Extensions.InverseLerp(Ladder.BottomMountPoint.position, Ladder.TopMountPoint.position, _playerBodyAbility.BodyTransform.position);

			if (t <= 0 || Unit.GetAbility<GroundMoveAbility>().IsGrounded || _inputManager.Jump.WasPressedThisFrame())
			{
				Dismount(Vector3.Lerp(Ladder.BottomMountPoint.position, Ladder.TopMountPoint.position, t));

				if (_inputManager.Jump.WasPressedThisFrame())
					_rigidbodyAbility.Rigidbody.linearVelocity = _playerHeadAbility.HeadFloatingAnchor.forward * 5;
			}
			else if (t >= 1)
			{
				Dismount(Ladder.TopDismountPoint.position);
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
			_rigidbodyAbility.Rigidbody.linearVelocity = Vector3.zero;
			Unit.GetAbility<ChangeHeightAbility>().Execute(_gameConfig.Player.ChangeHeight.StandConfig, 1);

			_smoothSpeed = 0;
			IsClimbing = true;
			_canDismount = false;

			float t = Vector3Extensions.InverseLerp(Ladder.BottomMountPoint.position, Ladder.TopMountPoint.position, _playerBodyAbility.BodyTransform.position);
			t = Mathf.Clamp(t, 0.1f, 0.9f);
			_targetPosition = Vector3.Lerp(Ladder.BottomMountPoint.position, Ladder.TopMountPoint.position, t);

			DOTween.To(() => _smoothSpeed, value => _smoothSpeed = value, _values.MaxSmoothDelta, _values.SmoothDeltaChangeTime).SetEase(Ease.InFlash);

			float distance = Vector3.Distance(_targetPosition, _playerBodyAbility.BodyTransform.position);

			_rigidbodyAbility.Rigidbody.interpolation = RigidbodyInterpolation.None;
			DOTween.Sequence()
				.Append(_playerBodyAbility.BodyTransform.DOMove(_targetPosition, distance * _values.MountTimeMultiplier).SetEase(Ease.InOutFlash))
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
			_playerBodyAbility.BodyTransform.position = targetPosition;
			_rigidbodyAbility.Rigidbody.gameObject.SetActive(true);

			float distance = Vector3.Distance(_playerHeadAbility.HeadStaticAnchor.position, _playerHeadAbility.HeadDetachedAnchor.position);
			Unit.GetAbility<ConnectHeadToBodyAbility>().Execute(distance * _values.DismountTimeMultiplier);

			Dismount();
		}
	}
}