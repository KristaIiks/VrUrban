using UnityEngine;
using UnityEngine.InputSystem;

namespace ToolsSystem
{
    [RequireComponent(typeof(AudioSource))]
    public class ToolsManager : MonoBehaviour
    {
        [SerializeField] private InputActionReference _windowInput;
        [SerializeField] private AudioClip _menuAudio;

        public static ToolsManager Instance;

        private ToolParams _currentTool;

        private AudioSource _audioSource;
        private ToolWindow[] _windows;

        protected virtual void Awake()
        {
            if(Instance != null) { Destroy(this); }

            Instance = this;
            _audioSource = GetComponent<AudioSource>();
            _windows = FindObjectsOfType<ToolWindow>();

            _windowInput.action.performed += (s) => WindowBtn();
        }

        public ToolWindow GetWindow(string _name)
        {
            foreach(ToolWindow _toolWindow in _windows)
            {
                if (_toolWindow._toolName == _name)
                {
                    return _toolWindow;
                }
            }
            return null;
        }

        private void WindowBtn()
        {
            if (_currentTool._tool == null) { return; }

            _currentTool._tool.CloseMenu();
            _audioSource.PlayOneShot(_menuAudio);
        }

    }

    [System.Serializable]
    public struct ToolParams
    {
        public Tool _tool;
        public BuildVariant[] _buildVariants;
    }
}
