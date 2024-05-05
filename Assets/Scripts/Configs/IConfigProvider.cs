namespace Scripts.Configs
{
	public interface IConfigProvider
	{
		T Get<T>(string uid = null) where T : Config;
		bool TryGet<T>(string uid, out T config) where T : Config;
	}
}