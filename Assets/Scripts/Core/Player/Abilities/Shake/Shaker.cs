using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Core.Player
{
	public class Shaker : MonoBehaviour
	{
		[SerializeField] private Transform _target;
		[SerializeField] private List<ShakerProcessor> _shakers;

		private void Awake()
		{
			foreach (ShakerProcessor shaker in _shakers)
			{
				shaker.Start();
			}
		}

		private void LateUpdate()
		{
			Vector3 resultPosition = Vector3.zero;
			Vector3 resultRotation = Vector3.zero;

			int index = 0;

			while (index < _shakers.Count)
			{
				ShakerProcessor shakerProcessor = _shakers[index];
				shakerProcessor.Update(out Vector3 position, out Vector3 rotation);
				
				resultPosition += position;
				resultRotation += rotation;

				if (shakerProcessor.State == ShakeProcessorState.Stopped)
				{
					_shakers.Remove(shakerProcessor);
					continue;
				}
				
				index++;
			}

			_target.localPosition = resultPosition;
			_target.localRotation = Quaternion.Euler(resultRotation);
		}

		public void StartShaker(ShakerProcessor shakerProcessor)
		{
			if (shakerProcessor == null)
			{
				Debug.LogError("Shaker is null");
				return;
			}
			
			_shakers.Add(shakerProcessor);
			shakerProcessor.Start();
		}

		public void StopShaker(ShakerProcessor shakerProcessor)
		{
			if (shakerProcessor == null)
			{
				Debug.LogError("Shaker is null");
				return;
			}

			if (!_shakers.Contains(shakerProcessor))
			{
				Debug.LogError("Shaker not found");
				return;
			}
			
			shakerProcessor.Stop();
		}
	}
}