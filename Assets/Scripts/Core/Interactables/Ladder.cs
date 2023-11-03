using DG.Tweening;
using Scripts.Core.Player;
using Scripts.Entities;
using Scripts.Input;
using Scripts.Reactive;
using Scripts.Utility;
using UnityEngine;
using Zenject;

namespace Scripts.Core
{
	public class Ladder : Ability
	{
		[SerializeField] private float _topDismountOffset;
		[SerializeField] private float _bottomDismountOffset;
		[SerializeField] private float _climbOffset;
		[SerializeField] private float _mountTimeMultiplier;
		[SerializeField] private float _dismountTimeMultiplier;
		[SerializeField] private AnimationCurve _dismountCurve;
		[SerializeField] private float _climbSpeed;
		[SerializeField] private float _smoothSpeedMax;
		[SerializeField] private float _smoothSpeedChangeTime;
		[SerializeField] private LayerMask _layerMask;
		[SerializeField] private TriggerProvider _triggerProvider;

		[Inject] private readonly InputManager _inputManager;

		private CompositeDisposable _disposable = new();
		private Unit _unit;
		private Vector3 _bottomTarget;
		private Vector3 _topTarget;
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
			if(!unit.HasAbility<PlayerMarkerAbility>())
				return;
			
			_unit = unit;

			PlayerHeadAbility playerHeadAbility = _unit.GetAbility<PlayerHeadAbility>();

			_bottomTarget = GetBottomMountPoint();
			_topTarget = GetTopMountPoint();

			Vector3 currentTarget = Vector3Extensions.ClosestPointOnLine(playerHeadAbility.HeadDetachedAnchor.position, _bottomTarget, _topTarget);
			
			_unit.GetAbility<MovementAbility>().AddDeactivator(this);
			_unit.GetAbility<LookAbility>().AddDeactivator(this);
			_unit.GetAbility<AttachHeadAbility>().AddDeactivator(this);

			_height = Mathf.InverseLerp(_bottomTarget.y, _topTarget.y, currentTarget.y);

			_smoothSpeed = 0;
			_isClimbing = true;
			_isDismounting = false;
			_canDismount = false;
			
			DOTween.To(() => _smoothSpeed, value => _smoothSpeed = value, _smoothSpeedMax, _smoothSpeedChangeTime).SetEase(Ease.InFlash);
			
			float distance = Vector3.Distance(currentTarget, playerHeadAbility.HeadDetachedAnchor.position);

			DOTween.Sequence()
				.Append(playerHeadAbility.HeadDetachedAnchor.DOMoveX(currentTarget.x, distance * _mountTimeMultiplier).SetEase(Ease.InOutFlash))
				.Join(playerHeadAbility.HeadDetachedAnchor.DOMoveZ(currentTarget.z, distance * _mountTimeMultiplier).SetEase(Ease.InOutFlash))
				.Join(playerHeadAbility.HeadDetachedAnchor.DORotateQuaternion(Quaternion.LookRotation(Unit.transform.forward), distance * _mountTimeMultiplier).SetEase(Ease.InOutFlash))
				.AppendCallback(() =>
				{
					_unit.GetAbility<LookAbility>().RemoveDeactivator(this);
					_canDismount = true;
				});
		}

		protected override void OnUpdate()
		{
			if (_isClimbing)
			{
				_height += _inputManager.Move.ReadValue<Vector2>().y * _climbSpeed * Time.deltaTime;

				if (!_canDismount)
				{
					_height = Mathf.Clamp01(_height);
				}

				Vector3 targetPosition = Vector3.Lerp(_bottomTarget, _topTarget, _height);
				PlayerHeadAbility playerHeadAbility = _unit.GetAbility<PlayerHeadAbility>();
				targetPosition.x = playerHeadAbility.HeadDetachedAnchor.position.x;
				targetPosition.z = playerHeadAbility.HeadDetachedAnchor.position.z;
				playerHeadAbility.HeadDetachedAnchor.position = Vector3.Lerp(playerHeadAbility.HeadDetachedAnchor.position, targetPosition, _smoothSpeed * Time.deltaTime);

				if (_canDismount)
				{
					if (_height < 0)
					{
						Dismount(GetBottomDismountPoint());
					}
				
					if (_height > 1)
					{
						Dismount(GetTopDismountPoint());
					}
				}
			}

			if (_isDismounting)
			{
				if (_dismountTimer < _dismountTime)
				{
					_dismountTimer += Time.deltaTime;
					
					PlayerHeadAbility playerHeadAbility = _unit.GetAbility<PlayerHeadAbility>();
					playerHeadAbility.HeadDetachedAnchor.position =
						Vector3.Lerp(playerHeadAbility.HeadDetachedAnchor.position, playerHeadAbility.HeadStaticAnchor.position, _dismountCurve.Evaluate(_dismountTimer / _dismountTime));
				}
				else
				{
					_isDismounting = false;
					_unit.GetAbility<AttachHeadAbility>().RemoveDeactivator(this);
				}
			}
		}

		private void Dismount(Vector3 targetPosition)
		{
			PlayerHeadAbility playerHeadAbility = _unit.GetAbility<PlayerHeadAbility>();
			PlayerBodyAbility playerBodyAbility = _unit.GetAbility<PlayerBodyAbility>();

			playerBodyAbility.CharacterController.enabled = false;
			playerBodyAbility.CharacterController.transform.position = targetPosition;
            
			_isDismounting = true;
			_isClimbing = false;
					
			float distance = Vector3.Distance(playerHeadAbility.HeadStaticAnchor.position, playerHeadAbility.HeadDetachedAnchor.position);
			
			_dismountTimer = 0;
			_dismountTime = distance * _dismountTimeMultiplier;
			
			_unit.GetAbility<MovementAbility>().RemoveDeactivator(this);
			
			playerBodyAbility.CharacterController.enabled = true;

			/*DOTween.Sequence()
				.Append(playerHeadAbility.HeadDetachedAnchor.DOMove(playerHeadAbility.HeadStaticAnchor.position, distance * _mountTimeMultiplier).SetEase(Ease.InOutFlash))
				.AppendCallback(() =>
				{
					_unit.GetAbility<AttachHeadAbility>().RemoveDeactivator(this);
				});*/
		}

		private Vector3 GetTopMountPoint()
		{
			return Unit.transform.position + Unit.transform.forward * _climbOffset + _unit.GetAbility<PlayerBodyAbility>().HalfHeight * Vector3.up;
		}

		private Vector3 GetTopDismountPoint()
		{
			return Unit.transform.position + Unit.transform.forward * _topDismountOffset;
		}

		private Vector3 GetBottomMountPoint()
		{
			Vector3 origin = Unit.transform.position + Unit.transform.forward * _climbOffset;
			float radius = _unit.GetAbility<PlayerBodyAbility>().Radius;
			Physics.SphereCast(origin, radius, Vector3.down, out RaycastHit hit, 100, _layerMask);
			return hit.point + hit.normal * radius + Vector3.down * radius + Vector3.up * _unit.GetAbility<PlayerBodyAbility>().Height;
		}

		private Vector3 GetBottomDismountPoint()
		{
			Vector3 origin = Unit.transform.position + Unit.transform.forward * _bottomDismountOffset;
			float radius = _unit.GetAbility<PlayerBodyAbility>().Radius;
			Physics.SphereCast(origin, radius, Vector3.down, out RaycastHit hit, 100, _layerMask);
			return hit.point + hit.normal * radius + Vector3.down * radius;
		}
	}
}