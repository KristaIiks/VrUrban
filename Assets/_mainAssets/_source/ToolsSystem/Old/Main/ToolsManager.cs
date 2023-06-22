using UnityEngine;
using UnityEngine.InputSystem;

namespace ToolsSystem.Old
{
    [RequireComponent(typeof(AudioSource))]
    public class ToolsManager : MonoBehaviour
    {
        [SerializeField] private InputActionReference CloseTools;
        [SerializeField] private AudioClip OnCloseBtn;

        public static ToolsManager Instance;

        private Tool _selectedTool;
        private AudioSource _audio;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                _audio = GetComponent<AudioSource>();
                CloseTools.action.performed += OnCloseButton;
                HandSelect.OnDeselect.AddListener(OnDeselect);
            }
        }

        public void OpenTool(Tool _tool, ToolParams _params)
        {
            Close();
            _selectedTool = _tool;

            _selectedTool.OpenTool(_params);
        }

        private void Close()
        {
            if (_selectedTool != null)
            {
                _selectedTool.CloseTool();
                _selectedTool = null;
            }
        }

        private void OnDeselect()
        {
            Close();
        }

        private void OnCloseButton(InputAction.CallbackContext cont)
        {
            HandSelect.Instance.Deselect();
            _audio.PlayOneShot(OnCloseBtn);
        }

    }
}
