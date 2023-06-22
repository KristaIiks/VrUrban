using UnityEngine;
using UnityEngine.Events;
using StudySystem;

namespace ToolsSystem.Old
{
    public class Buildable : Selectable
    {
        [SerializeField] private GameObject Standart;
        [SerializeField] private BuildVariant[] Variants;
        [SerializeField] private UnityEvent OnBuildChange = new UnityEvent();

        public override void Awake()
        {
            base.Awake();

            Params.BuildVariant = Variants;

            foreach (Transform item in transform)
            {
                if (item.gameObject != Standart)
                {
                    item.gameObject.SetActive(false);
                }
            }

        }

        public override bool Interact()
        {
            if (!base.Interact()) { return false; }

            ToolsManager.Instance.OpenTool(BuildTool.Instance, Params);
            return true;
        }

        public void ChangeBuild(GameObject _obj)
        {
            OnBuildChange?.Invoke();
            Standart.gameObject.SetActive(false);

            for (int i = 0; i < Variants.Length; i++)
            {
                Variants[i].Object.SetActive(false);
                Variants[i]._canSelect = true;
                Variants[i]._isSelected = false;
            }

            for (int i = 0; i < Variants.Length; i++)
            {
                if (Variants[i].Object == _obj)
                {
                    Variants[i].Object.SetActive(true);
                    Variants[i]._canSelect = false;
                    Variants[i]._isSelected = true;

                    Params.BuildVariant = Variants;
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

            StudyManager.Instance.StartNextStage(1f);
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
