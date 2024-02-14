using UnityEngine;

namespace ToolsSystem
{
    [RequireComponent(typeof(AudioSource))]
    public class SelectTool : Tool
    {
        [SerializeField] private AudioClip OnSelectBuild;

        public static SelectTool Instance;

        private void OnValidate()
        {
            _source ??= GetComponent<AudioSource>();
        }

        protected override void Awake()
        {
            base.Awake();

            Instance ??= this;
            gameObject.SetActive(false);
        }

        public void SelectBuild(GameObject _obj)
        {
            _selectedObject.GetComponent<Changable>().ChangeBuild(_obj);
            _source.PlayOneShot(OnSelectBuild);

            CloseMenu();
        }
    }
}
