using UnityEngine;

namespace ToolsSystem
{
    public class BuildWindow : ToolWindow
    {
        [SerializeField] private GameObject _window;

        [SerializeField] private GameObject _variantPrefab;
        [SerializeField] private Transform _content;

        public override void Open(ToolParams _params)
        {
            _window.SetActive(true);

            foreach (Transform item in _content)
            {
                Destroy(item.gameObject);
            }

            for (int i = 0; i < _params._buildVariants.Length; i++)
            {
                BuildSlot _tmp = Instantiate(_variantPrefab, _content, false).GetComponent<BuildSlot>();
                _tmp.Init(_params._buildVariants[i].Icon, _params._buildVariants[i].Object, _params._buildVariants[i]._canSelect, _params._buildVariants[i]._isSelected);
            }
        }

        public override void Close()
        {
            _window.SetActive(false);
        }
    }
}
