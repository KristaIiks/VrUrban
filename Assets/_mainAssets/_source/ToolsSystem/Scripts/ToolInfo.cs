using UnityEngine;

namespace ToolsSystem
{
	[CreateAssetMenu(fileName = "ToolInfo", menuName = "Tools/New Tool", order = 0)]
	public class ToolInfo : ScriptableObject
	{
		[field:SerializeField] public string ToolName { get; private set; }
		[field:SerializeField] public string ToolDescription { get; private set; }
		
		[field:SerializeField] public AudioClip SelectClip { get; private set; }
		[field:SerializeField] public AudioClip DeSelectClip { get; private set; }
	}
}