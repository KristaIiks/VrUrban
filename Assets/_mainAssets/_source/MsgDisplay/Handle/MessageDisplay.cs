using UnityEngine;

namespace MsgDisplay
{
	public class MessageDisplay
	{
		private GameObject _displayObject;
		private IDisplay _display;

		
		public void EnableDisplay(DisplaySettings settings)
		{
			if (_displayObject == null)
			{
				Debug.Log("Create new display");
				
				GameObject resource = null;
				switch(settings.Type)
				{
					case DisplayType.Standard:
						resource = Resources.Load<GameObject>("MsgDisplayStandard");
						break;
					case DisplayType.Developer:
						resource = Resources.Load<GameObject>("MsgDisplayDeveloper");
						break;
				}
				
				if (resource == null) { Debug.Log("Resource load failed"); return; }
				
				_displayObject = Object.Instantiate(resource);
				Object.DontDestroyOnLoad(_displayObject);
				
				_display = _displayObject.GetComponent<IDisplay>();
				
				Debug.Log("Display successful created");
			}
			
			Debug.Log("Configure msg display");
			
			/*
				TODO: configure display
			*/
			
			Debug.Log("Configure successful");
		}

		public void DisableDisplay()
		{
			if (_displayObject != null)
			{
				Object.Destroy(_displayObject);
			}
		}
		
		public void ChangeTarget(Transform target = null)
		{
			if (_display == null) { Debug.Log("Set target failed: No display"); return; }
			
			if (target == null)
			{
				// TODO: find player | if cant throw error
			}
			
			Debug.Log($"Configure new target {target.name}");
			// TODO: set target on display system
			Debug.Log("Target set successful");
		}
		
		public void SendMassage(string text, MsgCategoryEnum category = MsgCategoryEnum.Info)
		{
			if (_display == null || text == "") { return; }
			
			_display.ShowMessage(text, category);
		}
	}

}