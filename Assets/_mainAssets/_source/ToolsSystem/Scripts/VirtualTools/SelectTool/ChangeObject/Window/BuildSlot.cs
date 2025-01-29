using UnityEngine;
using UnityEngine.UI;

namespace ToolsSystem
{
	[RequireComponent(typeof(Button))]
	public class BuildSlot : MonoBehaviour
	{
		[SerializeField] private Image BuildIcon;
		[SerializeField] private GameObject SelectedIcon;
		[SerializeField] private GameObject BlockedIcon;

		private Button _selectButton;
		private ChangeObjectTool _tool;
		private int _id;

		private void OnValidate()
		{
			_selectButton ??= GetComponent<Button>();
		}

		public void Init(Sprite icon, int id, bool isBlocked, bool isSelected, ChangeObjectTool tool)
		{
			BuildIcon.sprite = icon;
			_tool = tool;
			_id = id;

			if (!isBlocked && !isSelected)
			{				
				_selectButton.onClick.AddListener(Select);
			}
			else
			{
				SelectedIcon.SetActive(isSelected);
				BlockedIcon.SetActive(isBlocked);
				
				_selectButton.interactable = false;
			}
		}

		private void Select()
		{
			_tool.SelectVariant(_id);
			_selectButton.onClick.RemoveListener(Select);
		}

		private void OnDestroy() => _selectButton.onClick.RemoveListener(Select);
	}
}