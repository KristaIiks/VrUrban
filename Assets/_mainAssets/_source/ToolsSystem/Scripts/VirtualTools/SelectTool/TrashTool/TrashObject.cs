using System;
using QuickOutline;
using SmartConsole;
using ToolsSystem;
using UnityEngine;

public sealed class TrashObject : Selectable
{
	[SerializeField] private float _decreaseTime = 5f;
	
	public event Action OnRemove;
	public event Action _studyEvent;
	
	private Vector3 _defaultScale;
	private bool _isDestroyed;
	private float _percent = 1f;

	protected override void OnValidate()
	{
		if (!_outline)
		{
			_outline = GetComponent<Outline>();
			_outline.enabled = false;
			_outline.OutlineWidth = 10f;
			_outline.OutlineColor = Color.red;
			_outline.BakeOutline = true;
			
			if (!gameObject.CompareTag(OBJECT_TAG)) { gameObject.tag = OBJECT_TAG; }
		}
		
		CanSelect = false;
		base.OnValidate();
	}

	private void Awake() => Restart(false);

	public override bool TryInteract(out bool canSelect, SelectFilter filter)
	{
		if (base.TryInteract(out canSelect, filter))
		{
			_outline.enabled = true;
			
			_isDestroyed = true;
			CanInteract = false;

			return true;
		}
		return false;
	}

	private void Update()
	{
		if (_isDestroyed)
		{
			_percent = Mathf.MoveTowards(_percent, 0, Time.deltaTime / _decreaseTime);
			transform.localScale = _defaultScale * _percent;
			
			if(_percent == 0) { Remove(); }
		}
	}

	private void Remove()
	{
		SConsole.Log("TrashObject", $"Remove - {gameObject.name}");
		
		gameObject.SetActive(false);
		OnRemove?.Invoke();
	}

	public override void StartDefaultStudy(Action OnComplete)
	{
		Restart(true);
		
		_studyEvent = () => { OnComplete.Invoke(); OnRemove -= _studyEvent; };
		OnRemove += _studyEvent;
	}

	public override void Skip()
	{
		Restart();
		Remove();
	}

	public override void Restart(bool canContinue = true)
	{
		_isDestroyed = false;
		CanInteract = canContinue;
		
		transform.localScale = _defaultScale;
	}
}