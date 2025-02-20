using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ToolsSystem
{
	public class BuildSlot : MonoBehaviour
	{
		[SerializeField] private TMP_Text NameText;
		[SerializeField] private TMP_Text DescriptionText;
		[SerializeField] private TMP_Text StatsText;
		[SerializeField] private TMP_Text PriceText;
		
		[SerializeField] private Image BuildIcon;
		[SerializeField] private GameObject SelectedIcon;
		[SerializeField] private GameObject BlockedIcon;
		[SerializeField] private Button _selectButton;

		private ChangeObjectTool _tool;
		private int _id;

		public void Init(ChangeVariant info, int id, ChangeObjectTool tool)
		{
			NameText.text = info.Name;
			DescriptionText.text = info.Description;
			
			StatsText.text = $"Комфортнойсть: {info.Rewards.Comfort}\n" + 
				$"Экология: {info.Rewards.Ecology}\n" +
				$"Безопасность: {info.Rewards.Security}\n" +
				$"Стоимость жилья: {info.Rewards.HousePrice}\n" +
				$"Эстетика: {info.Rewards.Beauty}\n";
			
			PriceText.text = info.Rewards.Money.ToString();
			
			BuildIcon.sprite = info.Icon;
			_tool = tool;
			_id = id;

			if (!info.IsBlocked && !info.IsSelected)
			{				
				_selectButton.onClick.AddListener(Select);
			}
			else
			{
				SelectedIcon.SetActive(info.IsSelected);
				BlockedIcon.SetActive(info.IsBlocked);
				
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