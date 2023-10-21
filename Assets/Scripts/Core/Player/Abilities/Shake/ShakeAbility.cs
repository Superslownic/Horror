using System.Collections.Generic;
using Scripts.Entities;
using UnityEngine;

namespace Scripts.Core.Player.Shake
{
	public class ShakeAbility : Ability
	{
		[SerializeField] private Transform _target;
		[SerializeField] private List<Shaker> _shakers;

		protected override void OnLateUpdate()
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

		public void AddShaker(Shaker shaker)
		{
			_shakers.Add(shaker);
		}
	}
}