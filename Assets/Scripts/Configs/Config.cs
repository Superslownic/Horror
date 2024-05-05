using Sirenix.OdinInspector;
using UnityEngine;

namespace Scripts.Configs
{
	public abstract class Config : ScriptableObject
	{
		[SerializeField,
		 EnableIf(nameof(_custom)),
		 HorizontalGroup("Guid")]
		public Uid Guid;

		[SerializeField,
		 OnValueChanged(nameof(UseClassNameValueChanged)),
		 HorizontalGroup("Guid", width: 20, PaddingLeft = 5), LabelWidth(50)]
		private bool _custom = true;

		private void UseClassNameValueChanged()
		{
			if (_custom)
			{
				Guid.Generate();
			}
			else
			{
				Guid.Value = GetType().Name;
			}
		}
	}
}