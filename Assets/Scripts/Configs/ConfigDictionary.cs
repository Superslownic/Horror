using System.Linq;
using Scripts.Utility;
using Sirenix.OdinInspector;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Scripts.Configs
{
	[CreateAssetMenu(menuName = CreateAssetMenuTabs.CONFIGS + nameof(ConfigDictionary))]
	public class ConfigDictionary : ScriptableDictionary<string, Config>
	{
		protected override string GetKey(Config value) => value.Guid;

		#if UNITY_EDITOR
		[Button]
		private void OnValidate()
		{
			_configs = AssetDatabase.FindAssets("t:Config").Select(guid =>
			{
				string path = AssetDatabase.GUIDToAssetPath(guid);
				return AssetDatabase.LoadAssetAtPath<Config>(path);
			}).ToArray();
			EditorUtility.SetDirty(this);
		}
		#endif
	}
}