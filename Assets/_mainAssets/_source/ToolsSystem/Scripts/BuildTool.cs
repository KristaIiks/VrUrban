using UnityEngine;

namespace ToolsSystem
{
    [RequireComponent(typeof(AudioSource))]
    public class BuildTool : Tool
    {
        [SerializeField] private AudioClip OnSelectBuild;

        public static BuildTool Instance;

        private void OnValidate()
        {
            _source ??= GetComponent<AudioSource>();
        }

        protected override void Awake()
        {
            base.Awake();

            Instance = this;
        }

        public void SelectBuild(GameObject _obj)
        {
            _selectedObject.GetComponent<Buildable>().ChangeBuild(_obj);
            _source.PlayOneShot(OnSelectBuild);

            CloseMenu();
        }
    }
}
