namespace Scripts.Utility
{
	public static class FloatExtensions
	{
		public static float Normalize(this float x, float min , float max)
		{
			return (x - min)/(max - min);
		}
	}
}