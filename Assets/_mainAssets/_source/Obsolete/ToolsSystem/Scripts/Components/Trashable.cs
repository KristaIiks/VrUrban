using StudySystem;
using ToolsSystem;
using UnityEngine;

public class Trashable : Selectable, IStudy
{
	protected override void OnValidate()
	{
		_params._openMenu = false;
		
		if(_outline == null)
		{
			_outline = GetComponent<Outline>();
			_outline.enabled = false;
			_outline.OutlineWidth = 10f;
			_outline.OutlineColor = Color.yellow;
		}
	}
	
	private bool _destroyed = false;
	public override void Interact()
	{
		base.Interact();
		
		_outline.OutlineColor = Color.red;
		Invoke("CompleteStudy", 5f);
		
		_destroyed = true;
		_canInteract = false;
	}
	
	private void Update()
	{
		if(_destroyed)
		{
			transform.localScale = new Vector3(transform.localScale.x - transform.localScale.x * .3f * Time.deltaTime, transform.localScale.y - transform.localScale.y * .3f * Time.deltaTime, transform.localScale.z - transform.localScale.z * .3f * Time.deltaTime);
		}
	}

	public override void Select() { }
	public override void Deselect() { }

	public void StartStudy()
	{
		_outline.enabled = true;
		_canInteract = true;
	}
	
	private void CompleteStudy()
	{
		StudyManager.Instance.StartNextStage(1f, gameObject);
		Destroy(gameObject);
	}
}
