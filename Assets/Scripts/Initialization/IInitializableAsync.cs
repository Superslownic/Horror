using System;
using Cysharp.Threading.Tasks;

namespace Scripts.Initialization
{
	public interface IInitializableAsync : IInitializableBase
	{
		UniTask Initialize(IProgress<float> progress);
	}
}