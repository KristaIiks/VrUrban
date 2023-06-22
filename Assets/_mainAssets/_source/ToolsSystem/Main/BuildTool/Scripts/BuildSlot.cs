using UnityEngine;
using UnityEngine.UI;

public class BuildSlot : MonoBehaviour
{
    [SerializeField] private Image BuildIcon;
    [SerializeField] private GameObject MarkIcon;

    private GameObject _building;
    private Button _selectButton;

    private void Awake()
    {
        _selectButton = GetComponent<Button>();
    }

    public void Init(Sprite _icon, GameObject _build, bool _canInteract, bool _isSelected)
    {
        BuildIcon.sprite = _icon;
        _building = _build;

        if (_canInteract)
        {
            _selectButton.onClick.AddListener(Select);
        }
        else
        {
            _selectButton.interactable = false;
        }

        if (_isSelected)
        {
            MarkIcon.SetActive(true);
        }

    }

    private void Select()
    {
        BuildTool.Instance.GetComponent<BuildTool>().SelectBuild(_building);
        _selectButton.onClick.RemoveListener(Select);
    }

    private void OnDestroy()
    {
        _selectButton.onClick.RemoveListener(Select);
    }

}
