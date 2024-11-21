using System;
using UnityEngine;

namespace ToolsSystem
{
	[RequireComponent(typeof(Collider))]
	public abstract class SprayPoint : BaseToolObject
	{
		public abstract event Action OnPaint;
		public abstract bool Paint();
	}
}
