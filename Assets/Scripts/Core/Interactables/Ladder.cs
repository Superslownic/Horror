using DG.Tweening;
using Scripts.Core.Player;
using Scripts.Entities;
using Scripts.Input;
using Scripts.Reactive;
using UnityEngine;
using Zenject;

namespace Scripts.Core
{
	public class Ladder : Ability
	{
		[SerializeField] private float _topOffset;
		[SerializeField] private float _bottomOffset;
		[SerializeField] private LayerMask _layerMask;
		[SerializeField] private TriggerProvider _topTriggerProvider;
		[SerializeField] private TriggerProvider _bottomTriggerProvider;

		[Inject] private readonly InputManager _inputManager;

		private CompositeDisposable _disposable = new();
		private Unit _unit;
		private Vector3 _bottomTarget;
		private Vector3 _topTarget;
		private float _height;
		private bool _isClimbing;

		protected override void OnInitialize()
		{
			//_topTriggerProvider.OnEnter.AddListener(HandleTopTriggerEnter).AddTo(_disposable);
			_bottomTriggerProvider.OnEnter.AddListener(HandleBottomTriggerEnter).AddTo(_disposable);
		}

		protected override void OnDispose()
		{
			_disposable.Dispose();
		}

		private void HandleBottomTriggerEnter(Unit unit)
		{
			if(!unit.HasAbility<PlayerMarkerAbility>())
				return;
			
			_unit = unit;

			PlayerBodyAbility playerBodyAbility = _unit.GetAbility<PlayerBodyAbility>();
			PlayerHeadAbility playerHeadAbility = _unit.GetAbility<PlayerHeadAbility>();

			_bottomTarget = GetBottomPoint() + Vector3.up * playerBodyAbility.Height;
			_topTarget = Unit.transform.position + Unit.transform.forward * _bottomOffset + Vector3.up * playerBodyAbility.Height * 0.5f;
			//Vector3 end = Unit.transform.position + Unit.transform.forward * _topOffset + Vector3.up * playerBodyAbility.Height;
			
			_unit.GetAbility<MovementAbility>().AddDeactivator(this);
			_unit.GetAbility<LookAbility>().AddDeactivator(this);
			_unit.GetAbility<AttachHeadAbility>().AddDeactivator(this);

			_isClimbing = true;
			
			DOTween.Sequence()
				.Append(playerHeadAbility.HeadDetachedAnchor.DOMove(_bottomTarget, 1).SetEase(Ease.InCubic))
				.Join(playerHeadAbility.HeadDetachedAnchor.DORotateQuaternion(Quaternion.LookRotation(Unit.transform.forward), 1).SetEase(Ease.InOutCubic))
				.AppendCallback(() => _unit.GetAbility<LookAbility>().RemoveDeactivator(this))
				.AppendCallback(() => _isClimbing = true);
			/*.Append(playerHeadAbility.HeadDetachedAnchor.DOMove(mid, 2).SetEase(Ease.Linear))
			.AppendCallback(() => _unit.GetAbility<LookAbility>().AddDeactivator(this))
			.Append(playerHeadAbility.HeadDetachedAnchor.DOMove(end, 1.5f).SetEase(Ease.InOutCubic))
			.Join(playerHeadAbility.HeadDetachedAnchor.DORotateQuaternion(Quaternion.LookRotation(Unit.transform.forward), 1.5f).SetEase(Ease.InOutCubic))
			.AppendCallback(() => _unit.GetAbility<LookAbility>().RemoveDeactivator(this));*/
		}

		protected override void OnUpdate()
		{
			if (_isClimbing)
			{
				_height += _inputManager.Move.ReadValue<Vector2>().y * Time.deltaTime;
				_height = Mathf.Clamp01(_height);
				PlayerHeadAbility playerHeadAbility = _unit.GetAbility<PlayerHeadAbility>();
				playerHeadAbility.HeadDetachedAnchor.position = Vector3.Lerp(playerHeadAbility.HeadDetachedAnchor.position, Vector3.Lerp(_bottomTarget, _topTarget, _height), Time.deltaTime);
			}
		}

		private Vector3 GetTopPoint()
		{
			return Unit.transform.position + Unit.transform.forward * _topOffset;
		}
		
		private Vector3 GetBottomPoint()
		{
			Vector3 origin = Unit.transform.position + Unit.transform.forward * _bottomOffset;
			float radius = _unit.GetAbility<PlayerBodyAbility>().CharacterController.radius;
			Physics.SphereCast(origin, radius, Vector3.down, out RaycastHit hit, 100, _layerMask);
			return hit.point + hit.normal * radius + Vector3.down * radius;
		}
	}
}