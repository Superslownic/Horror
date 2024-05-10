using Scripts.Units;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.UI;

namespace Scripts.Core.Player.Breath
{
	public class PlayerScreenFadeAbility : Ability
	{
		[SerializeField] private float _maxValue;

		private Image _blackScreen;
		private Vignette _vignette;
		private float _startIntensity;
		private float _startSmoothness;
		private float _startRoundness;

		protected override void OnInitialize()
		{
			base.OnInitialize();

			_blackScreen = GameObject.Find("BlackScreen").GetComponent<Image>();
			if (GameObject.Find("GlobalVolume").GetComponent<Volume>().profile.TryGet(out Vignette vignette))
			{
				_vignette = vignette;

				_startIntensity = vignette.intensity.value;
				_startSmoothness = vignette.smoothness.value;
				_startRoundness = vignette.roundness.value;

				_vignette.intensity.max = _maxValue;
				_vignette.smoothness.max = _maxValue;
				_vignette.roundness.max = _maxValue;
			}
		}

		public void SetValue(float normalizedValue)
		{
			_vignette.intensity.value = Mathf.Lerp(_startIntensity, _maxValue, normalizedValue);
			_vignette.smoothness.value = Mathf.Lerp(_startSmoothness, _maxValue, normalizedValue);
			_vignette.roundness.value = Mathf.Lerp(_startRoundness, _maxValue, normalizedValue);

			Color color = _blackScreen.color;
			color.a = normalizedValue;
			_blackScreen.color = color;
		}
	}
}