using UnityEngine;

namespace ToolsSystem
{
	[CreateAssetMenu(fileName = "ToolInfo", menuName = "Tools/New Tool", order = 0)]
	public class ToolInfo : ScriptableObject
	{
		[field:SerializeField] public string ToolName { get; private set; }
		[field:SerializeField, TextArea(1, 2)] public string ToolDescription { get; private set; }
		[field:SerializeField] public InToolInteractionMode InteractionMode { get; private set; }
		[field:SerializeField] public AudioClip SelectToolClip { get; private set; }
		[field:SerializeField] public AudioClip DeSelectToolClip { get; private set; }
	}
}