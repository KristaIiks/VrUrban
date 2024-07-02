using UnityEngine;

namespace MsgDisplay
{
	[System.Serializable]
	public struct DisplaySettings
	{
		public int MaxMessages {get; }
		public MsgCategoryEnum Filter {get; }
		public float TimeToFadeout {get; }
		public DisplayType Type {get; }
		public AudioClip ReceiveClip {get; }
		public float MaxAngle {get; }
		public float MoveSpeed {get; }
		
		public DisplaySettings(int maxMsgs = 5, MsgCategoryEnum filter = MsgCategoryEnum.Info | MsgCategoryEnum.Error, float fadeTime = 3f, DisplayType displayType = DisplayType.Standard, AudioClip audio = null, float maxAngle = 60f, float speed = 10f)
		{
			MaxMessages = maxMsgs;
			Filter = filter;
			TimeToFadeout = fadeTime;
			Type = displayType;
			ReceiveClip = audio;
			MaxAngle = maxAngle;
			MoveSpeed = speed;
		}
	}
}
