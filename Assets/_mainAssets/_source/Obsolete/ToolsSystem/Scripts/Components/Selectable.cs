using UnityEngine;
using UnityEngine.Events;

namespace ToolsSystem
{
	[RequireComponent(typeof(Outline))]
	public class Selectable : MonoBehaviour
	{
		public bool _canInteract = true;

		[HideInInspector]
		public ToolParams _params;

		public UnityEvent OnSelect = new UnityEvent();
		public UnityEvent OnDeselect = new UnityEvent();

		private bool _isSelected = false;
		private Outline _outline;

		private void OnValidate()
		{
			_outline ??= GetComponent<Outline>();
		}


		public virtual void Interact()
		{
			if (!_canInteract) { return; }
		}

		public virtual void Select()
		{
			if (_isSelected) { return; }

			OnSelect?.Invoke();
			_isSelected = true;
			
			_outline.enabled = true;
		}

		public virtual void Deselect()
		{
			if (!_isSelected) { return; }

			OnDeselect?.Invoke();
			_isSelected = false;

			_outline.enabled = false;
		}
		
		[ContextMenu("Init")]
		public void GenerateColliders()
		{
			gameObject.tag = "SelectObject";
			
			MeshFilter[] objects = transform.GetComponentsInChildren<MeshFilter>(true);
			
			foreach (MeshFilter mesh in objects)
			{
				if (mesh.GetComponent<Collider>() == null)
				{
					mesh.gameObject.AddComponent<MeshCollider>();
					mesh.gameObject.layer = LayerMask.NameToLayer("Selectable");
				}
			}
			Debug.Log($"Успешно добавлено {objects.Length} коллайдера");
		}
	}
}
