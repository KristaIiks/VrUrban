using TMPro;
using UnityEngine;

namespace StudySystem
{
	public class FinishRes : MonoBehaviour
	{
		[SerializeField] private TMP_Text percentText;
		[SerializeField] private TMP_Text moneyText;
		[SerializeField] private TMP_Text timeText;
		[SerializeField] private TMP_Text metPriceText;
		[SerializeField] private TMP_Text comfortText;
		[SerializeField] private TMP_Text ecologyText;
		[SerializeField] private TMP_Text securityText;
		[SerializeField] private TMP_Text housePriceText;
		[SerializeField] private TMP_Text beautyText;
		
		public void UpdateResults(Branch branch)
		{
			BranchResult result = branch.result;
			
			percentText.text = Mathf.CeilToInt(100 - (result.Mistakes / 11f)) + "%";
			moneyText.text = result.Money.ToString();
			
			if (result.Time >= 60f)
				timeText.text = Mathf.CeilToInt((float)result.Time / 60f) + "м";
			else
				timeText.text = result.Time + "с";
				
			//housePriceText.text = result.Stats.HousePrice + "р";
			comfortText.text = Mathf.Clamp(result.Stats.Comfort, 0, 100) + "/100";
			ecologyText.text = Mathf.Clamp(result.Stats.Ecology, 0, 100) + "/100";
			securityText.text = Mathf.Clamp(result.Stats.Security, 0, 100) + "/100";
			housePriceText.text = Mathf.Clamp(result.Stats.HousePrice, 0, 100) + "/100";
			beautyText.text = Mathf.Clamp(result.Stats.Beauty, 0, 100) + "/100";
		}
	}
}
