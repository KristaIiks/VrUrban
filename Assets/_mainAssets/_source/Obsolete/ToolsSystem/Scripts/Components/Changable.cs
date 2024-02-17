using StudySystem;
using UnityEngine;
using UnityEngine.Events;

namespace ToolsSystem
{
	public class Changable : Selectable, IStudy
	{
		[SerializeField] private GameObject Standart;
		[SerializeField] private BuildVariant[] _variants;

		public UnityEvent OnBuildChange = new UnityEvent();

		private void Awake()
		{
			_params._buildVariants = _variants;
			_params._openMenu = true;
		}

		public void ChangeBuild(GameObject _obj)
		{
			OnBuildChange?.Invoke();

			if (Standart.activeSelf) { Standart.gameObject.SetActive(false); }

			for (int i = 0; i < _variants.Length; i++)
			{
				_variants[i].Object.SetActive(false);
				_variants[i]._canSelect = true;
				_variants[i]._isSelected = false;
			}

			for (int i = 0; i < _variants.Length; i++)
			{
				if (_variants[i].Object == _obj)
				{
					_variants[i].Object.SetActive(true);
					_variants[i]._canSelect = false;
					_variants[i]._isSelected = true;

					_params._buildVariants = _variants;
					return;
				}
			}
		}

		public void StartStudy()
		{
			_canInteract = true;
			OnBuildChange.AddListener(StopStudy);
		}

		private void StopStudy()
		{
			OnBuildChange.RemoveListener(StopStudy);

			StudyManager.Instance.StartNextStage(1f, gameObject);
		}
	}

	[System.Serializable]
	public struct BuildVariant
	{
		public Sprite Icon;
		public GameObject Object;

		public bool _canSelect;
		public bool _isSelected;
	}
}
