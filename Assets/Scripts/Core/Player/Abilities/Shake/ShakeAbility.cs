using System.Collections.Generic;
using Scripts.Entities;
using Sirenix.OdinInspector;
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

			int index = 0;

			while (index < _shakers.Count)
			{
				Shaker shaker = _shakers[index];
				shaker.Update(out Vector3 position, out Vector3 rotation);
				
				resultPosition += position;
				resultRotation += rotation;

				if (shaker.Type == ShakeProcessorType.Finite && shaker.State == ShakeProcessorState.Stopped)
				{
					_shakers.Remove(shaker);
					continue;
				}
				
				index++;
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