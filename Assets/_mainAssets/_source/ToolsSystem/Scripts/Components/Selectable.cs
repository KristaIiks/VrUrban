using UnityEngine;
using UnityEngine.Events;

namespace ToolsSystem
{
    public class Selectable : MonoBehaviour
    {
        public bool _canInteract = true;

        [HideInInspector]
        public ToolParams _params;

        public UnityEvent OnSelect = new UnityEvent();
        public UnityEvent OnDeselect = new UnityEvent();

        private bool _isSelected = false;

        public virtual void Interact()
        {
            if (!_canInteract) { return; }
        }

        public virtual void Select()
        {
            OnSelect?.Invoke();
            _isSelected = true;
            int _layer = LayerMask.NameToLayer("Outline");

            gameObject.layer = _layer;
            foreach (var item in transform.GetComponentsInChildren<MeshRenderer>())
            {
                item.gameObject.layer = _layer;
            }
        }

        public virtual void Deselect()
        {
            if (_isSelected) { return; }

            OnDeselect?.Invoke();
            _isSelected = false;

            int _layer = LayerMask.NameToLayer("Selectable");

            gameObject.layer = _layer;
            foreach (var item in transform.GetComponentsInChildren<MeshRenderer>())
            {
                item.gameObject.layer = _layer;
            }
        }
    }
}
