using System.Collections.Generic;
using SmartConsole;
using UnityEngine;

namespace ToolsSystem
{
	public class ChangeWindow : MonoBehaviour
	{
		private const string LOG_TAG = "ToolWindow";
		
		[SerializeField] private BuildSlot _variantPrefab;
		[SerializeField] private Transform _content;

		public void Open(List<ChangeVariant> variants, Transform UIPosition, ChangeObjectTool tool)
		{
			foreach (Transform item in _content)
			{
				Destroy(item.gameObject);
			}

			for (int i = 0; i < variants.Count; i++)
			{
				if (variants[i].IsHidden)
					continue;
				
				Instantiate(_variantPrefab, _content, false).Init(
					variants[i],
					i,
					tool
				);
			}
			
			SConsole.Log(LOG_TAG, "Open change window");
			
			transform.SetParent(UIPosition);
			
			transform.localPosition = Vector3.zero;
			transform.localRotation = Quaternion.identity;
			
			gameObject.SetActive(true);
		}

		public void Close()
		{
			SConsole.Log(LOG_TAG, "Close change window");
			gameObject.SetActive(false);
		}
	}
}