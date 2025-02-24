using System;
using StudySystem;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ToolsSystem
{
	public sealed class SlideVariants : MonoBehaviour, IStudyObject
	{
		[SerializeField] private GameObject Window;
		[SerializeField] private TMP_Text PercentText;
		[SerializeField] private Button Button;
		[SerializeField] private Slider PercentSlider;
		
		[SerializeField] private SlideObject[] Objects;
		
		private UnityAction _studyAction;
		
		private void Awake()
		{
			Restart(false);
			PercentSlider.onValueChanged.AddListener(UpdateModels);
		}
		
		private void UpdateModels(float value)
		{
			float percent = value / PercentSlider.maxValue;
			PercentText.text = Mathf.RoundToInt(percent * 100).ToString() + "%";
			
			for (int i = 0; i < Objects.Length; i++)
			{
				Objects[i].Select((float)i / Objects.Length < percent);
			}
		}
		
		public void StartDefaultStudy(Action OnComplete)
		{
			Restart(true);
			
			_studyAction = new UnityAction(() => { OnComplete.Invoke(); Button.onClick.RemoveListener(_studyAction); });
			Button.onClick.AddListener(_studyAction);
		}
		
		public void Skip()
		{
			UpdateModels(UnityEngine.Random.Range(0, PercentSlider.maxValue));
		}
		
		public void Restart(bool canContinue = true)
		{
			Window.SetActive(canContinue);
			
			UpdateModels(.5f);
		}
	}
}