using System.Collections;
using TMPro;
using UnityEngine;

public class textwriter : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;

    public void DisplayText(string text) => StartCoroutine(Displaying(text));

    private IEnumerator Displaying(string text)
    {
        _text.gameObject.SetActive(true);
        _text.text = text;

        yield return new WaitForSeconds(3f);

        _text.gameObject.SetActive(true);
    }
}
