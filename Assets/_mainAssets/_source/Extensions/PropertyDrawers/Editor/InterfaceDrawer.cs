using UnityEditor;
using UnityEngine;

namespace StudySystem
{
	public class InterfaceAttribute : PropertyAttribute
	{
		public InterfaceAttribute(params System.Type[] types)
		{
			m_types = types;
		}

		public System.Type[] Types
		{
			get { return m_types; }
		}
		private System.Type[] m_types;
	}
	#if UNITY_EDITOR
	[CustomPropertyDrawer(typeof(InterfaceAttribute))]
	public class InterfaceDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			var constraint = attribute as InterfaceAttribute;
			Event evt = Event.current;
			
			if (DragAndDrop.objectReferences.Length > 0 && position.Contains(evt.mousePosition))
			{
				var draggedObject = DragAndDrop.objectReferences[0] as GameObject;

				if(!CheckType(draggedObject, constraint.Types))
				{
					DragAndDrop.visualMode = DragAndDropVisualMode.Rejected;
				}
			}

			// If a value was set through other means (e.g. ObjectPicker)
			if(property.objectReferenceValue != null)
			{
				GameObject obj = property.objectReferenceValue as GameObject;
				
				if(!CheckType(obj, constraint.Types))
				{
					property.objectReferenceValue = null;
				}
			}
				
			property.objectReferenceValue = EditorGUI.ObjectField(position, label, property.objectReferenceValue, typeof(GameObject), true);
		}
		
		private bool CheckType(GameObject obj, System.Type[] types)
		{
			if (obj == null) { return false; }
			
			foreach (var type in types)
			{
				if(obj.GetComponent(type) != null)
				{
					return true;
				}
			}
			return false;
		}
	}
	#endif
}