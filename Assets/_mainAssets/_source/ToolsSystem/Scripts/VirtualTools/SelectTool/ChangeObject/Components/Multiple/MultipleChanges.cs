using SmartConsole;
using UnityEngine;

namespace ToolsSystem
{
	public sealed class MultipleChanges : Changeable
	{
		[Space(25)]
		[SerializeField] private MultipleChanges[] Objects;
		
		private void OnEnable()
		{
			if (Variants.Count < Objects.Length + 1) { SConsole.Log("Tool: Change tool", "Need more build variants for multiple change"); return; }
			
			foreach (var obj in Objects)
			{
				obj.OnObjectChanged += HideVariant;
			}
		}
		
		private void OnDisable()
		{
			foreach (var obj in Objects)
			{
				obj.OnObjectChanged -= HideVariant;
			}
		}
	}
}