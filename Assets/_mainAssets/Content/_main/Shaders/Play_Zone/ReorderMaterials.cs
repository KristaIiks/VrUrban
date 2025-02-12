using UnityEngine;

public class ReorderMaterials : MonoBehaviour
{
	private void Awake()
	{
		MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>(true);
		
		foreach (var item in renderers)
		{
			item.material.renderQueue = 3001;
		}
	}
}
