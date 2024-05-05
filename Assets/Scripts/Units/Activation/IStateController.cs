namespace Scripts.Behaviour
{
	public interface IStateController
	{
		bool Add(object obj);
		bool Remove(object obj);
		bool GetState();
	}
}