using DG.Tweening;
using Scripts.Configs;
using Scripts.Configs.Player;
using Scripts.Core.Player;
using Scripts.Core.Player.States;
using Scripts.Input;
using Scripts.Reactive;
using Scripts.Units;
using Scripts.Utility.Extensions;
using UnityEngine;
using Zenject;

namespace Scripts.Core
{
	public class LadderClimbingAbility : Ability
	{
		public DisposableAction OnLadderDetected { get; } = new();
		
		[SerializeField] private TriggerProvider _triggerProvider;

		[Inject] private readonly InputManager _inputManager;
		[Inject] private readonly GameConfig _gameConfig;

		private Ladder _ladder;
		private CompositeDisposable _disposable = new();
		private LadderValuesConfig _values;
		private float _smoothSpeed;
		private float _height;
		private float _dismountTime;
		private float _dismountTimer;
		private bool _isClimbing;
		private bool _isDismounting;
		private bool _canDismount;

		protected override void OnInitialize()
		{
			_triggerProvider.OnEnter.AddListener(HandleTriggerEnter).AddTo(_disposable);
		}

		protected override void OnDispose()
		{
			_disposable.Dispose();
		}

		private void HandleTriggerEnter(Unit unit)
		{
			if (unit.TryGetAbility(out Ladder ladder))
			{
				_ladder = ladder;
				OnLadderDetected.Invoke();
			}
		}

		protected override void OnUpdate()
		{
			if (_isClimbing)
			{
				float input = _inputManager.Move.ReadValue<Vector2>().y;
				
				Unit.GetAbility<HeadBobAbility>().OverrideMagnitude(Mathf.Abs(input));
				
				_height += input * _values.ClimbSpeed * Time.deltaTime;

				if (!_canDismount)
				{
					_height = Mathf.Clamp01(_height);
				}

				Vector3 targetPosition = Vector3.Lerp(_ladder.BottomMountPoint.position, _ladder.TopMountPoint.position, _height);
				PlayerHeadAbility playerHeadAbility = Unit.GetAbility<PlayerHeadAbility>();
				targetPosition.x = playerHeadAbility.HeadDetachedAnchor.position.x;
				targetPosition.z = playerHeadAbility.HeadDetachedAnchor.position.z;
				playerHeadAbility.HeadDetachedAnchor.position = Vector3.Lerp(playerHeadAbility.HeadDetachedAnchor.position, targetPosition, _smoothSpeed * Time.deltaTime);

				if (_canDismount)
				{
					if (_height < 0)
					{
						Dismount(_ladder.BottomDismountPoint.position);
					}
				
					if (_height > 1)
					{
						Dismount(_ladder.TopDismountPoint.position);
					}
				}
			}

			if (_isDismounting)
			{
				if (_dismountTimer < _dismountTime)
				{
					_dismountTimer += Time.deltaTime;
					
					PlayerHeadAbility playerHeadAbility = Unit.GetAbility<PlayerHeadAbility>();
					playerHeadAbility.HeadDetachedAnchor.position =
						Vector3.Lerp(playerHeadAbility.HeadDetachedAnchor.position, playerHeadAbility.HeadStaticAnchor.position, _gameConfig.Player.Ladder.DismountCurve.Evaluate(_dismountTimer / _dismountTime));
				}
				else
				{
					_isDismounting = false;
					Unit.GetAbility<AttachHeadAbility>().RemoveDeactivator(this);
				}
			}
		}

		private void Mount()
		{
			_values = Unit.GetAbility<PlayerStateMachine>().CurrentState is StandingRunState
				? _gameConfig.Player.Ladder.RunValues
				: _gameConfig.Player.Ladder.DefaultValues;

			PlayerHeadAbility playerHeadAbility = Unit.GetAbility<PlayerHeadAbility>();

			Unit.GetAbility<GroundMoveAbility>().AddDeactivator(this);
			Unit.GetAbility<LookAbility>().AddDeactivator(this);
			Unit.GetAbility<AttachHeadAbility>().AddDeactivator(this);
			Unit.GetAbility<HeadBobAbility>().AddDeactivator(this);
			Unit.GetAbility<HeadBobAbility>().OverrideConfig(_values.ShakeConfig);
			Unit.GetAbility<LeanAbility>().AddDeactivator(this);

			_smoothSpeed = 0;
			_isClimbing = true;
			_isDismounting = false;
			_canDismount = false;

			Vector3 currentTarget = Vector3Extensions.ClosestPointOnLine(playerHeadAbility.HeadDetachedAnchor.position, _ladder.BottomMountPoint.position, _ladder.TopMountPoint.position);

			_height = Mathf.InverseLerp(_ladder.BottomMountPoint.position.y, _ladder.TopMountPoint.position.y, currentTarget.y);

			DOTween.To(() => _smoothSpeed, value => _smoothSpeed = value, _values.MaxSmoothDelta, _values.SmoothDeltaChangeTime).SetEase(Ease.InFlash);
			
			float distance = Vector3.Distance(currentTarget, playerHeadAbility.HeadDetachedAnchor.position);

			DOTween.Sequence()
				.Append(playerHeadAbility.HeadDetachedAnchor.DOMoveX(currentTarget.x, distance * _values.MountTimeMultiplier).SetEase(Ease.InOutFlash))
				.Join(playerHeadAbility.HeadDetachedAnchor.DOMoveZ(currentTarget.z, distance * _values.MountTimeMultiplier).SetEase(Ease.InOutFlash))
				.Join(playerHeadAbility.HeadDetachedAnchor.DORotateQuaternion(Quaternion.LookRotation(_ladder.RotationTransform.forward), distance * _values.MountTimeMultiplier).SetEase(Ease.InOutFlash))
				.AppendCallback(() =>
				{
					Unit.GetAbility<LookAbility>().RemoveDeactivator(this);
					_canDismount = true;
				});
		}

		private void Dismount(Vector3 targetPosition)
		{
			PlayerHeadAbility playerHeadAbility = Unit.GetAbility<PlayerHeadAbility>();
			PlayerBodyAbility playerBodyAbility = Unit.GetAbility<PlayerBodyAbility>();

			playerBodyAbility.Collider.enabled = false;
			playerBodyAbility.Collider.transform.position = targetPosition;
            
			_isDismounting = true;
			_isClimbing = false;
					
			float distance = Vector3.Distance(playerHeadAbility.HeadStaticAnchor.position, playerHeadAbility.HeadDetachedAnchor.position);
			
			_dismountTimer = 0;
			_dismountTime = distance * _values.DismountTimeMultiplier;
			
			Unit.GetAbility<GroundMoveAbility>().RemoveDeactivator(this);
			Unit.GetAbility<HeadBobAbility>().RemoveDeactivator(this);
			Unit.GetAbility<HeadBobAbility>().CancelOverride();
			Unit.GetAbility<LeanAbility>().RemoveDeactivator(this);

			playerBodyAbility.Collider.enabled = true;
		}
	}
}