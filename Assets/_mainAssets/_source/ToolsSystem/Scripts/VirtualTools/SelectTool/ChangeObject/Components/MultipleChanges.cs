using SmartConsole;
using UnityEngine;

namespace ToolsSystem
{
	public sealed class MultipleChanges : Changeable
	{
		[SerializeField] private MultipleChanges[] Objects;
		
		protected override void Awake()
		{
			base.Awake();
			if (Variants.Count < Objects.Length + 1) { SConsole.Log("Tool: Change tool", "Need more build variants for multiple change"); return; }
			
			foreach (var obj in Objects)
			{
				obj.OnObjectChanged += RemoveVariant;
			}
		}
		
		public void RemoveVariant(int id) => Variants.RemoveAt(id);
	}
}