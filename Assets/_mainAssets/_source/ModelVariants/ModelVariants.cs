using UnityEngine;

public class ModelVariants : MonoBehaviour
{
	[SerializeField] private GameObject[] Objects;
	
	private void Start()
	{
		foreach (GameObject item in Objects)
		{
			item.SetActive(false);
		}
		
		Objects[Random.Range(0, Objects.Length)].SetActive(true);
	}
}