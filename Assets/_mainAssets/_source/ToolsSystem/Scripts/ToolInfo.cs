using UnityEngine;

namespace ToolsSystem
{
	[CreateAssetMenu(fileName = "ToolInfo", menuName = "Tools/New Tool", order = 0)]
	public class ToolInfo : ScriptableObject
	{
		[field:SerializeField] public string ToolName { get; private set; }
		[field:SerializeField] public string ToolDescription { get; private set; }
		[field:SerializeField] public ToolMode ToolMode { get; private set; }
		
		// TODO: make this fields custom variants and make default clips for all standard tools
		[field:SerializeField] public AudioClip SelectClip { get; private set; }
		[field:SerializeField] public AudioClip DeSelectClip { get; private set; }
	}
}