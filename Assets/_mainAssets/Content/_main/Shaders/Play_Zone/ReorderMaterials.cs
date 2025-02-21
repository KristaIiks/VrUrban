using System.Linq;
using UnityEngine;

[ExecuteInEditMode]
public class ReorderMaterials : MonoBehaviour
{
	[SerializeField] private Renderer[] IgnoreList;
	
	private void Awake()
	{
		Renderer[] renderers = GetComponentsInChildren<Renderer>(true);
		
		foreach (var item in renderers)
		{
			if (IgnoreList.Contains(item)) { continue; }
			
			#if UNITY_EDITOR
			Material tempMaterial = new Material(item.sharedMaterial);
			tempMaterial.renderQueue = 2502;
			item.sharedMaterial = tempMaterial;
			#else
			item.material.renderQueue = 2502;
			#endif
		}
	}
}
