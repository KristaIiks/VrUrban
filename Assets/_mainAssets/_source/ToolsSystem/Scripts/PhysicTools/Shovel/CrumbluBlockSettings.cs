using UnityEngine;
using UnityEngine.VFX;

namespace ToolsSystem
{
	[CreateAssetMenu(fileName = "CrumblyBlockSettings", menuName = "Tools/Objects/Settings", order = 0)]
	public sealed class CrumblyBlockSettings : ScriptableObject
	{
		[field: SerializeField] public Material Material { get; private set;}
		[field: SerializeField] public VisualEffect DigEffect { get; private set;}
		[field: SerializeField] public AudioClip DigSound { get; private set;}
		[field: SerializeField] public Vector2 PitchRange { get; private set;}
	}
}