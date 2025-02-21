using StudySystem;
using UnityEngine;

[System.Serializable]
public class ChangeVariant
{
	[field:SerializeField] public string Name { get; private set; }
	[field:SerializeField, TextArea(1, 4)] public string Description { get; private set; }
	[field:SerializeField] public Sprite Icon { get; private set; }
	[field:SerializeField] public GameObject Object { get; private set; }
	[field:SerializeField] public RewardStats Rewards { get; private set; }
	
	[HideInInspector] public bool IsBlocked;
	[HideInInspector] public bool IsHidden;
	[HideInInspector] public bool IsSelected;
}