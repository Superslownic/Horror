using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Scripts.Configs
{
	public class ConfigJsonConverter : JsonConverter<Config>
	{
		private readonly IConfigProvider _configProvider;

		public ConfigJsonConverter(IConfigProvider configProvider)
		{
			_configProvider = configProvider;
		}

		public override bool CanWrite => true;
		public override bool CanRead => true;

		public override void WriteJson(JsonWriter writer, Config value, JsonSerializer serializer)
		{
			writer.WriteValue(value.Guid);
		}

		public override Config ReadJson(JsonReader reader, Type objectType, Config existingValue, bool hasExistingValue, JsonSerializer serializer)
		{
			JToken token = JToken.Load(reader);
			string value = token.ToObject<string>();

			if (string.IsNullOrEmpty(value))
				return null;

			if (!_configProvider.TryGet(value, out Config config))
				return null;

			return config;
		}
	}
}