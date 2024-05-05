using System;
using Cysharp.Threading.Tasks;
using Scripts.Initialization;
using Scripts.Reflection;
using UnityEngine;

namespace Scripts.Configs
{
	public class ConfigProvider : IConfigProvider, IInitializableAsync
	{
		private ConfigDictionary _configDictionary = new();

		public async UniTask Initialize(IProgress<float> progress)
		{
			_configDictionary = (ConfigDictionary) await Resources
				.LoadAsync<ConfigDictionary>("ConfigDictionary")
				.ToUniTask(progress);
		}

		public T Get<T>(string uid = null) where T : Config
		{
			if (string.IsNullOrEmpty(uid))
				uid = TypeCache<T>.Name;

			return (T) _configDictionary[uid];
		}

		public bool TryGet<T>(string uid, out T config) where T : Config
		{
			if (string.IsNullOrEmpty(uid))
				uid = TypeCache<T>.Name;

			bool result = _configDictionary.TryGet(uid, out Config value);
			config = (T)value;
			return result;
		}
	}
}