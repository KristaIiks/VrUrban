using UnityEngine;

namespace ToolsSystem
{
	[CreateAssetMenu(fileName = "SolidBlockSettings", menuName = "Tools/Objects/Settings/SolidBlock", order = 1)]
	public class SolidBlockSettings : ScriptableObject
	{
		[field: SerializeField] public float Health { get; private set; }
		[field: SerializeField] public AudioClip BreakSound { get; private set; }
	}
}