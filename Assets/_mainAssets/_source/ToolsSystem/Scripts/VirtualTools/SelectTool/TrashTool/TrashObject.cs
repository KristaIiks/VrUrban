using UnityEngine;
using ToolsSystem;
using QuickOutline;
using SmartConsole;

public sealed class TrashObject : Selectable
{
	[SerializeField] private float _decreaseTime = 5f;
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
		}
		CanSelect = false;
		base.OnValidate();
	}

	private void Awake()
	{
		_defaultScale = transform.localScale;
	}

	public override bool TryInteract(out bool canSelect)
	{
		if (base.TryInteract(out canSelect))
		{
			_outline.enabled = true;
			
			_isDestroyed = true;
			CanInteract = false;
			
			SConsole.Log("TrashObject", $"Remove - {gameObject.name}");

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
		gameObject.SetActive(false);
	}
}