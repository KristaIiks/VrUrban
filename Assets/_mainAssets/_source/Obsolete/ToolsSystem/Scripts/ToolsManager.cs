using UnityEngine;
using UnityEngine.InputSystem;

namespace ToolsSystem
{
	[RequireComponent(typeof(AudioSource))]
	public class ToolsManager : MonoBehaviour
	{
		[SerializeField] private InputActionReference _windowInput;
		[SerializeField] private AudioClip _menuAudio;

		[SerializeField] private GameObject _interactHand;
		[SerializeField] private GameObject _handModel;

		public static ToolsManager Instance;

		private Tool _currentTool;

		private AudioSource _audioSource;
		private ToolWindow[] _windows;

		private void Awake()
		{
			Instance = this;
			_audioSource = GetComponent<AudioSource>();
			_windows = FindObjectsOfType<ToolWindow>();

			_windowInput.action.performed += (s) => WindowBtn();
		}

		public void DeselectTool()
		{
			_handModel.SetActive(true);
			_interactHand.SetActive(true);

			SelectTool.Instance.gameObject.SetActive(false);
			GazTool.Instance.gameObject.SetActive(false);
			BuildTool.Instance.gameObject.SetActive(false);
		}

		public void SetTool(Tool _tool)
		{
			_handModel.SetActive(true);

			_interactHand.SetActive(false);

			SelectTool.Instance.gameObject.SetActive(false);
			GazTool.Instance.gameObject.SetActive(false);
			BuildTool.Instance.gameObject.SetActive(false);

			switch (_tool)
			{
				case SelectTool:
					SelectTool.Instance.gameObject.SetActive(true);
					break;
				case GazTool:
					GazTool.Instance.gameObject.SetActive(true);
					_handModel.SetActive(false);
					break;
				case BuildTool:
					BuildTool.Instance.gameObject.SetActive(true);
					break;
			}

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
			if (_currentTool == null) { return; }

			_currentTool.CloseMenu();
			_audioSource.PlayOneShot(_menuAudio);
		}

	}

	[System.Serializable]
	public struct ToolParams
	{
		public Tool _tool;
		public bool _openMenu;
		public BuildVariant[] _buildVariants;
	}
}
