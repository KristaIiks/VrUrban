using UnityEngine;
using SmartConsole;

namespace ToolsSystem
{
	public class ChangeWindow : MonoBehaviour
	{
		private const string LOG_TAG = "ToolWindow";
		
		[SerializeField] private BuildSlot _variantPrefab;
		[SerializeField] private Transform _content;

		public void Open(ChangeVariant[] variants, ChangeObjectTool tool)
		{
			foreach (Transform item in _content)
			{
				Destroy(item.gameObject);
			}

			for (int i = 0; i < variants.Length; i++)
			{
				BuildSlot _tmp = Instantiate(_variantPrefab, _content, false);
				_tmp.Init(variants[i].Icon, i, variants[i].IsBlocked, variants[i].IsSelected, tool);
			}
			
			SConsole.Log(LOG_TAG, "Open change window");
			gameObject.SetActive(true);
		}

		public void Close()
		{
			SConsole.Log(LOG_TAG, "Close change window");
			gameObject.SetActive(false);
		}
	}
}