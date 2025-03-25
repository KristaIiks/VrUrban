using System;
using SmartConsole;
using ToolsSystem;
using UnityEngine;

public sealed class TrashObject : Selectable
{
	[SerializeField] private float DecreaseTime = 5f;
	[SerializeField] private Vector3 DefaultScale;
	
	public event Action OnRemove;
	
	private event Action _studyEvent;
	
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
		DefaultScale = transform.localScale;
		
		base.OnValidate();
	}

	private void Awake() => Restart(false);

	public override bool TryInteract(out bool canSelect, SelectFilter filter)
	{
		if (base.TryInteract(out canSelect, filter))
		{
			CanSelect = false;
			CanInteract = false;
			
			if (SelectOutline)
				SelectOutline.OutlineColor = Color.red;
			
			_isDestroyed = true;

			return true;
		}
		return false;
	}
	
	public override void Deselect()
	{
		_isSelected = false;
	}
	
	private void Update()
	{
		if (!_isDestroyed)
			return;
		
		_percent = Mathf.MoveTowards(_percent, 0, Time.deltaTime / DecreaseTime);
		transform.localScale = DefaultScale * _percent;
		
		if(_percent == 0)
			Remove();
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
		
		transform.localScale = DefaultScale;
	}
}