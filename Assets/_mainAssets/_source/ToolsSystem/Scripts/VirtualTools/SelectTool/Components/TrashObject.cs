using UnityEngine;
using ToolsSystem;
using QuickOutline;
using SmartConsole;

public sealed class TrashObject : Selectable
{
	private const float DECRIES_SPEED = .3f;
	private bool _isDestroyed;

	protected override void OnValidate()
	{
		if (!_outline)
		{
			_outline = GetComponent<Outline>();
			_outline.enabled = false;
			_outline.OutlineWidth = 10f;
			_outline.OutlineColor = Color.yellow;
		}
		CanSelect = false;
		base.OnValidate();
	}

	public override bool TryInteract(out bool canSelect)
	{
		if (base.TryInteract(out canSelect))
		{
			_outline.OutlineColor = Color.red;

			_isDestroyed = true;
			CanInteract = false;
			// TODO: remove
			SConsole.Log("Tool: TrashTool", "Remove - " + gameObject.name);

			return true;
		}
		return false;
	}

	// TODO: update remove anim to percent animation
	private void Update()
	{
		if (_isDestroyed)
		{
			transform.localScale = new Vector3(
				transform.localScale.x - transform.localScale.x * DECRIES_SPEED * Time.deltaTime,
				transform.localScale.y - transform.localScale.y * DECRIES_SPEED * Time.deltaTime,
				transform.localScale.z - transform.localScale.z * DECRIES_SPEED * Time.deltaTime
			);
		}
	}

	private void Remove()
	{
		gameObject.SetActive(false);
	}
}