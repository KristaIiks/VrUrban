using UnityEngine;
using UnityEngine.Events;
using StudySystem;

namespace ToolsSystem.Old
{
    public class Selectable : MonoBehaviour
    {
        public bool _canInteract = true;

        public UnityEvent OnSelect = new UnityEvent();
        public UnityEvent OnDeselect = new UnityEvent();

        private bool _isSelected = false;
        protected ToolParams Params;

        public virtual void Awake()
        {
            Params.Selected = gameObject;
        }

        public virtual bool Interact()
        {
            if (!CanInteract()) { return false; }

            Select();
            return true;
        }

        public bool CanInteract()
        {
            return _canInteract;
        }

        private void Select()
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

        public void Deselect()
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
