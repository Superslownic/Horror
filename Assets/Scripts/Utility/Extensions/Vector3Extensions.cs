using UnityEngine;

namespace Scripts.Utility
{
	public static class Vector3Extensions
	{
		public static Vector3 ClosestPointOnLine(float px, float py, float pz, float ax, float ay, float az, float bx, float by, float bz)
		{
			float apx = px - ax;
			float apy = py - ay;
			float apz = pz - az;
			float abx = bx - ax;
			float aby = by - ay;
			float abz = bz - az;
			float abMag = abx * abx + aby * aby + abz * abz;
			
			if(abMag < Mathf.Epsilon)
				return new Vector3(ax, ay, az);
			
			abMag = Mathf.Sqrt(abMag);
			abx /= abMag;
			aby /= abMag;
			abz /= abMag;
			
			float mu = abx * apx + aby * apy + abz * apz;
			if(mu < 0) return new Vector3(ax, ay, az);
			if(mu > abMag) return new Vector3(bx, by, bz);
			return new Vector3(ax + abx * mu, ay + aby * mu, az + abz * mu);
		}
		
		public static Vector3 ClosestPointOnLine(Vector3 p, Vector3 a, Vector3 b)
		{
			return ClosestPointOnLine(p.x, p.y, p.z, a.x, a.y, a.z, b.x, b.y, b.z);
		}
		
		public static Vector3 SetY(this Vector3 vector, float value)
		{
			vector.y = value;
			return vector;
		}

		public static Vector3 ChangeY(this Vector3 vector, float value)
		{
			vector.y += value;
			return vector;
		}
	}
}