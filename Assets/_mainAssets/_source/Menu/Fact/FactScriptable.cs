using UnityEngine;

[CreateAssetMenu(menuName = "Facts/New Fact")]
public class FactScriptable : ScriptableObject
{
	[field: SerializeField, Multiline(3)] public string Text {get; private set;}
	[field: SerializeField] public AudioClip Audio {get; private set;}
}