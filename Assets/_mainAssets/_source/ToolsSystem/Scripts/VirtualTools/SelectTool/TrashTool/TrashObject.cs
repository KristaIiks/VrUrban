using System;
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
		if (!SelectOutline && TryGetComponent(out SelectOutline))
		{
			SelectOutline.enabled = false;
			SelectOutline.OutlineWidth = 10f;
			SelectOutline.OutlineColor = Color.yellow;
			SelectOutline.BakeOutline = true;
		}
		
		CanSelect = false;
		_defaultScale = transform.localScale;
		
		base.OnValidate();
	}

	private void Awake() => Restart(false);

	public override bool TryInteract(out bool canSelect, SelectFilter filter)
	{
		if (base.TryInteract(out canSelect, filter))
		{
			CanInteract = false;
			
			if (SelectOutline) { SelectOutline.OutlineColor = Color.red; }
			_isDestroyed = true;

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
		
		SelectOutline.OutlineColor = Color.yellow;
		SelectOutline.enabled = canContinue;
		
		transform.localScale = _defaultScale;
	}
}