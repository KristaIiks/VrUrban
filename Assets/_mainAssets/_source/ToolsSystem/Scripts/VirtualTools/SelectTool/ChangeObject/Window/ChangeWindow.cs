using UnityEngine;
using SmartConsole;
using System.Collections.Generic;

namespace ToolsSystem
{
	public class ChangeWindow : MonoBehaviour
	{
		private const string LOG_TAG = "ToolWindow";
		
		[SerializeField] private BuildSlot _variantPrefab;
		[SerializeField] private Transform _content;

		public void Open(List<ChangeVariant> variants, Vector3 UIPosition, ChangeObjectTool tool)
		{
			foreach (Transform item in _content)
			{
				Destroy(item.gameObject);
			}

			for (int i = 0; i < variants.Count - 1; i++)
			{
				if (variants[i].IsHidden) { continue; }
				
				Instantiate(_variantPrefab, _content, false).Init(
					variants[i].Icon, 
					i, 
					variants[i].IsBlocked, 
					variants[i].IsSelected, 
					tool
				);
			}
			
			SConsole.Log(LOG_TAG, "Open change window");
			
			transform.position = UIPosition;
			gameObject.SetActive(true);
		}

		public void Close()
		{
			SConsole.Log(LOG_TAG, "Close change window");
			
			transform.position = Vector3.zero;
			gameObject.SetActive(false);
		}
	}
}