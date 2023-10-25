using System.Collections.Generic;
using Scripts.Entities;
using UnityEngine;

namespace Scripts.Core.Player.Shake
{
	public class ShakeAbility : Ability
	{
		[SerializeField] private Transform _target;
		[SerializeField] private List<Shaker> _shakers;

		protected override void OnInitialize()
		{
			foreach (Shaker shaker in _shakers)
			{
				shaker.Start();
			}
		}

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

				if (shaker.State == ShakeProcessorState.Stopped)
				{
					_shakers.Remove(shaker);
					continue;
				}
				
				index++;
			}

			_target.localPosition = resultPosition;
			_target.localRotation = Quaternion.Euler(resultRotation);
		}

		public void StartShaker(Shaker shaker)
		{
			if (shaker == null)
			{
				Debug.LogError("Shaker is null");
				return;
			}
			
			_shakers.Add(shaker);
			shaker.Start();
		}

		public void StopShaker(Shaker shaker)
		{
			if (shaker == null)
			{
				Debug.LogError("Shaker is null");
				return;
			}

			if (!_shakers.Contains(shaker))
			{
				Debug.LogError("Shaker not found");
				return;
			}
			
			shaker.Stop();
		}
	}
}