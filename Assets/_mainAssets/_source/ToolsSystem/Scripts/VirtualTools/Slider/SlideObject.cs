using UnityEngine;

namespace ToolsSystem
{
	public class SlideObject : MonoBehaviour
	{
		[SerializeField] private GameObject FirstObject;
		[SerializeField] private GameObject SecondObject;
		
		public void Select(bool first)
		{
			FirstObject.SetActive(first);
			SecondObject.SetActive(!first);
		}
		
		public void Reset()
		{
			if (Random.Range(0, 100) <= 50)
			{
				FirstObject.SetActive(true);
				SecondObject.SetActive(false);
			}
			else
			{
				FirstObject.SetActive(false);
				SecondObject.SetActive(true);
			}
		}
	}
}
