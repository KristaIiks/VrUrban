using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class FactsManager : MonoBehaviour
{
	[SerializeField] private FactScriptable[] _facts;
	[SerializeField] private TMP_Text _factText;
	[SerializeField] private int _textRange = 15;
	
	private List<FactScriptable> _factHistories = new List<FactScriptable>();

	private AudioSource _audioSource;

	private void OnValidate()
	{
		_audioSource ??= GetComponent<AudioSource>();
	}

	[ContextMenu("Generate fact")]
	public void NextFact()
	{
		_audioSource.Stop();
		
		if(_factHistories.Count - 1 == _facts.Length) { _factHistories.Clear(); }
		
		FactScriptable _currentFact = null;
		
		while(_currentFact == null)
		{
			FactScriptable _temp = _facts[Random.Range(0, _facts.Length)];
			
			if(!_factHistories.Equals(_temp))
			{
				_currentFact = _temp;
				_factHistories.Add(_currentFact);
			}
		}
		
		_factText.text = TruncateString(_currentFact.Text, _textRange) + "...";
		
		if(_currentFact.Audio != null) { _audioSource.PlayOneShot(_currentFact.Audio); }
	}
	
	private string TruncateString(string _input, int _length)
	{
		if(_input.Length <= _length || _input == "") { return _input; }
		
		return _input.Substring(0, _length);
	}
}