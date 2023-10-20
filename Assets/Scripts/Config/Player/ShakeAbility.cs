using Scripts.Entities;
using UnityEngine;

namespace Scripts.Config.Player
{
	public class ShakeAbility : Ability
	{
		[SerializeField] private Transform _target;
		[SerializeReference] private Shaker[] _shakers;

		protected override void OnUpdate()
		{
			Vector3 resultPosition = Vector3.zero;
			Vector3 resultRotation = Vector3.zero;
			
			foreach (Shaker shaker in _shakers)
			{
				shaker.Update(out Vector3 position, out Vector3 rotation);
				resultPosition += position;
				resultRotation += rotation;
			}
			
			_target.localPosition = resultPosition;
			_target.localRotation = Quaternion.Euler(resultRotation);
		}
	}
}