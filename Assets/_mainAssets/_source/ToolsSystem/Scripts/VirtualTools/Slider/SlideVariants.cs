using System;
using StudySystem;
using UnityEngine;
using UnityEngine.UI;

namespace ToolsSystem
{
	public sealed class SlideVariants : MonoBehaviour, IStudyObject
	{
		[SerializeField] private GameObject Window;
		[SerializeField] private Slider PercentSlider;
		[SerializeField] private SlideObject[] Objects;
		
		private void Awake()
		{
			Restart(false);
			PercentSlider.onValueChanged.AddListener(UpdateModels);
		}
		
		private void UpdateModels(float value)
		{
			float percent = value / PercentSlider.maxValue;
			
			for (int i = 0; i < Objects.Length; i++)
			{
				Objects[i].Select((float)i / Objects.Length <= percent);
			}
		}
		
		public void StartDefaultStudy(Action OnComplete)
		{
			Restart(true);
		}
		
		public void Skip()
		{
			UpdateModels(UnityEngine.Random.Range(0, PercentSlider.maxValue));
		}
		
		public void Restart(bool canContinue = true)
		{
			Window.SetActive(canContinue);
			
			foreach (SlideObject obj in Objects)
			{
				obj.Reset();
			}
		}
	}
}