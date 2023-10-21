using TMPro;
using UnityEngine;

namespace Scripts.Utility
{
	public class FPSCounter : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI fpsText;
		
		private float deltaTime = 0;
		
		private void Update()
		{
			deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
			float fps = 1.0f / deltaTime;

			if (fpsText != null)
			{
				fpsText.text = $"FPS: {Mathf.Ceil(fps)}";
			}
		}
	}
}