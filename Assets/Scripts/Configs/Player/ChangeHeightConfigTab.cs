using System;
using Scripts.Core.Player;

namespace Scripts.Configs.Player
{
	[Serializable]
	public class ChangeHeightConfigTab
	{
		public ChangeHeightConfig StandConfig;
		public ChangeHeightConfig CrouchConfig;
		public ChangeHeightConfig SwimConfig;
		public ChangeHeightConfig LadderConfig;
		public float Duration;
	}
}