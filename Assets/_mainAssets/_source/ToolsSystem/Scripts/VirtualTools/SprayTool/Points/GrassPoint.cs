using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ToolsSystem
{
	[RequireComponent(typeof(MeshRenderer))]
	public sealed class GrassPoint : SprayPoint
	{
		[SerializeField] private Material GroundMaterial;
		[SerializeField] private Material GrassMaterial;
		[SerializeField] GrassPoint[] NearPoints;
		[HideInInspector] public bool CanPaint { get; private set; }

		public override event Action OnPaint;
		private event Action _studyEvent;

		private MeshRenderer _meshRenderer;
		
		private void OnValidate()
		{
			_meshRenderer ??= GetComponent<MeshRenderer>();
		}
		
		private void Awake() => Restart();
		
		public override bool Paint()
		{
			if (!CanPaint) { return false; }
			
			_meshRenderer.material = GrassMaterial;
			CanPaint = false;
			OnPaint?.Invoke();
			StartCoroutine(Expansion());
			
			return true;
		}
		private IEnumerator Expansion()
		{
			while (true)
			{
				yield return new WaitForSeconds(2f);
				
				if (NearPoints.All((point) => !point.CanPaint)) { yield break; }
				
				List<GrassPoint> points = NearPoints.Where((point) => point.CanPaint).ToList();
				points[UnityEngine.Random.Range(0, points.Count - 1)].Paint();
			}
		}

		public override void StartDefaultStudy(Action OnComplete = null)
		{
			Restart(true);
			
			_studyEvent = () => { OnComplete.Invoke(); OnPaint -= _studyEvent; };
			OnPaint += _studyEvent;
		}

		public override void Skip()
		{
			Restart();
			Paint();
		}

		public override void Restart(bool canContinue = true)
		{			
			CanPaint = canContinue;
			_meshRenderer.material = GroundMaterial;
		}
	}
}
